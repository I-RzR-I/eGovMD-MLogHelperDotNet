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

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Result;
using DomainCommonExtensions.DataTypeExtensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace MLogHelperDotNet.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A HTTP client extension.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    internal static class HttpClientExtension
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>A HttpClient extension method that HTTP post.</summary>
        /// <remarks>RzR, 04-Dec-23.</remarks>
        /// <param name="client">The client to act on.</param>
        /// <param name="registerUrl">URL of the register.</param>
        /// <param name="content">The content.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        /// =================================================================================================
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
                return Result<string>.Failure(e.Message)
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A HttpClient extension method that HTTP post asynchronous.</summary>
        /// <remarks>RzR, 04-Dec-23.</remarks>
        /// <param name="client">The client to act on.</param>
        /// <param name="registerUrl">URL of the register.</param>
        /// <param name="content">The content.</param>
        /// <returns>The HTTP post.</returns>
        /// =================================================================================================
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
                return Result<string>.Failure(e.Message)
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A HttpClient extension method that HTTP get.</summary>
        /// <remarks>RzR, 04-Dec-23.</remarks>
        /// <param name="client">The client to act on.</param>
        /// <param name="queryUrl">URL of the query.</param>
        /// <param name="query">The query.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        /// =================================================================================================
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
                return Result<string>.Failure(e.Message)
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A HttpClient extension method that HTTP get asynchronous.</summary>
        /// <remarks>RzR, 04-Dec-23.</remarks>
        /// <param name="client">The client to act on.</param>
        /// <param name="queryUrl">URL of the query.</param>
        /// <param name="query">The query.</param>
        /// <returns>The HTTP get.</returns>
        /// =================================================================================================
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
                return Result<string>.Failure(e.Message)
                    .WithError(e);
            }
        }
    }
}