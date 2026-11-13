// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-04 22:47
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-04 23:51
// ***********************************************************************
//  <copyright file="HttpClientExtension.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.ResultMessage;
using RzR.ResultMessage.Abstractions;
using RzR.ResultMessage.Models;
using RzR.Extensions.Domain.Validation;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace MLogHelperDotNet.Extensions
{
    /// <summary>
    ///     A HTTP client extension.
    /// </summary>
    internal static class HttpClientExtension
    {
        /// <summary>
        ///     A HttpClient extension method that HTTP post.
        /// </summary>
        /// <param name="client">The client to act on.</param>
        /// <param name="registerUrl">URL of the register.</param>
        /// <param name="content">The content.</param>
        /// <returns>
        ///     An IResult&lt;string&gt;
        /// </returns>
        internal static IResult<string> HttpPost(this HttpClient client, string registerUrl, string content)
        {
            client.ThrowArgIfNull(nameof(client));
            registerUrl.ThrowArgIfNull(nameof(registerUrl));
            content.ThrowArgIfNull(nameof(content));

            try
            {
                string resultMessage;
                using (var response = client.PostAsync(registerUrl, new StringContent(content)).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();
                    using var result = response.Content;
                    resultMessage = result.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                return Result<string>.Success(resultMessage);
            }
            catch (Exception e)
            {
                return ToFailure(e);
            }
        }

        /// <summary>
        ///     A HttpClient extension method that HTTP post asynchronous.
        /// </summary>
        /// <param name="client">The client to act on.</param>
        /// <param name="registerUrl">URL of the register.</param>
        /// <param name="content">The content.</param>
        /// <returns>
        ///     The HTTP post.
        /// </returns>
        internal static async Task<IResult<string>> HttpPostAsync(this HttpClient client, string registerUrl, string content)
        {
            client.ThrowArgIfNull(nameof(client));
            registerUrl.ThrowArgIfNull(nameof(registerUrl));
            content.ThrowArgIfNull(nameof(content));

            try
            {
                string resultMessage;
                using (var response = await client.PostAsync(registerUrl, new StringContent(content)))
                {
                    response.EnsureSuccessStatusCode();
                    using var result = response.Content;
                    resultMessage = await result.ReadAsStringAsync();
                }

                return Result<string>.Success(resultMessage);
            }
            catch (Exception e)
            {
                return ToFailure(e);
            }
        }

        /// <summary>
        ///     A HttpClient extension method that HTTP get.
        /// </summary>
        /// <param name="client">The client to act on.</param>
        /// <param name="queryUrl">URL of the query.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        ///     An IResult&lt;string&gt;
        /// </returns>
        internal static IResult<string> HttpGet(this HttpClient client, string queryUrl, string query)
        {
            client.ThrowArgIfNull(nameof(client));
            queryUrl.ThrowArgIfNull(nameof(queryUrl));
            query.ThrowArgIfNull(nameof(query));

            try
            {
                string resultMessage;
                using (var response = client.GetAsync(queryUrl + query).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();
                    using var content = response.Content;
                    resultMessage = content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                return Result<string>.Success(resultMessage);
            }
            catch (Exception e)
            {
                return ToFailure(e);
            }
        }

        /// <summary>
        ///     A HttpClient extension method that HTTP get asynchronous.
        /// </summary>
        /// <param name="client">The client to act on.</param>
        /// <param name="queryUrl">URL of the query.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        ///     The HTTP get.
        /// </returns>
        internal static async Task<IResult<string>> HttpGetAsync(this HttpClient client, string queryUrl, string query)
        {
            client.ThrowArgIfNull(nameof(client));
            queryUrl.ThrowArgIfNull(nameof(queryUrl));
            query.ThrowArgIfNull(nameof(query));

            try
            {
                string resultMessage;
                using (var response = await client.GetAsync(queryUrl + query))
                {
                    response.EnsureSuccessStatusCode();
                    using var content = response.Content;
                    resultMessage = await content.ReadAsStringAsync();
                }

                return Result<string>.Success(resultMessage);
            }
            catch (Exception e)
            {
                return ToFailure(e);
            }
        }

        /// <summary>
        ///     Maps a request exception to a failure result that carries only a fixed, non-sensitive
        ///     message.
        /// </summary>
        /// <remarks>
        ///     The exception details are intentionally not copied into the result.
        /// </remarks>
        /// <param name="e">
        ///     The exception thrown while sending the request or reading the response.
        /// </param>
        /// <returns>
        ///     An IResult&lt;string&gt; failure.
        /// </returns>
        private static IResult<string> ToFailure(Exception e)
        {
            string text;
            if (e is TaskCanceledException || e is OperationCanceledException)
                text = "The MLog service request timed out or was canceled.";
            else if (e is HttpRequestException)
                text = "The MLog service request failed (connection or non-success HTTP status).";
            else
                text = "The MLog service request failed.";

            return Result<string>.Failure(new MessageDataModel("MLog request failed", text));
        }
    }
}