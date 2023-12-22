// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-08 00:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-19 21:59
// ***********************************************************************
//  <copyright file="MLogRestClientService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************


#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using DomainCommonExtensions.ArraysExtensions;
using MLogHelperDotNet.Abstractions;
using MLogHelperDotNet.Extensions;
using MLogHelperDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.Options;
using System.Net.Http;
using AggregatedGenericResultMessage.Models;
#endif

#if NET45
using MNotifyHelperDotNet.Configurations.Options;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using AggregatedGenericResultMessage.Models;
#endif

#endregion

namespace MLogHelperDotNet.Services
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A log client.  MLog client</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    public class MLogRestClientService : IMLogEndpointClientService, IDisposable
    {
        /// <summary>
        ///     Remote logging options
        /// </summary>
        private readonly RemoteMLogOptions _remoteMLogOptions;

        private HttpClient _client;
        private Uri _registerUrl;

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks></remarks>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        public MLogRestClientService(IOptions<RemoteMLogOptions> options)
        {
            _remoteMLogOptions = options.Value;

            InitRequired();
        }

#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks></remarks>
        /// <param name="options">[in,out] Options for controlling the operation.</param>
        /// =================================================================================================
        public MLogRestClientService(ref RemoteMLogOptions options)
        {
            _remoteMLogOptions = options;

            InitRequired();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Default constructor.</summary>
        /// <remarks>RzR, 12-Dec-23.</remarks>
        /// =================================================================================================
        public MLogRestClientService()
        {
#if NET45
            _remoteMLogOptions = MLogPostConfigureOptionsNetFramework.InitOptions();

            InitRequired();
#endif
        }

        /// <inheritdoc />
        public void Dispose() => _client?.Dispose();

        /// <inheritdoc />
        public IResult<string> RegisterEvent(string jsonEvent)
        {
            if (jsonEvent.IsNullOrWhiteSpace())
            {
                return Result<string>
                    .Failure($"Cannot register empty event, ${nameof(jsonEvent)}");
            }

            var registerResult = _client.HttpPost(_registerUrl.ToString(), jsonEvent);

            return registerResult;
        }

        /// <inheritdoc />
        public IResult<string> RegisterEventBatch(IReadOnlyCollection<string> jsonEvents)
        {
            if (jsonEvents.IsNullOrEmptyEnumerable())
            {
                return Result<string>
                    .Failure(new MessageDataModel("No events provided for registration",
                        $"No events provided for registration, ${nameof(jsonEvents)}"));
            }

            return RegisterEvent(string.Join(Environment.NewLine, jsonEvents));
        }

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventAsync(string jsonEvent)
        {
            if (jsonEvent.IsNullOrWhiteSpace())
            {
                return Result<string>
                    .Failure(new MessageDataModel("Cannot register empty event",
                        $"Cannot register empty event, ${nameof(jsonEvent)}"));
            }

            var registerResult = await _client.HttpPostAsync(_registerUrl.ToString(), jsonEvent);

            return registerResult;
        }

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventBatchAsync(IReadOnlyCollection<string> jsonEvents)
        {
            if (jsonEvents.IsNullOrEmptyEnumerable())
            {
                return Result<string>
                    .Failure(new MessageDataModel("No events provided for registration",
                        $"No events provided for registration, ${nameof(jsonEvents)}"));
            }

            return await RegisterEventAsync(string.Join(Environment.NewLine, jsonEvents));
        }

        /// <inheritdoc />
        public IResult<string> RegisterEvent(MLogEventDto logEvent)
            => RegisterEvent(logEvent.ToString());

        /// <inheritdoc />
        public IResult<string> RegisterEventBatch(IEnumerable<MLogEventDto> logEvents)
            => RegisterEventBatch(logEvents.Select(x => x.ToString()).ToList());

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventAsync(MLogEventDto logEvent)
            => await RegisterEventAsync(logEvent.ToString());

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventBatchAsync(IEnumerable<MLogEventDto> logEvents)
            => await RegisterEventBatchAsync(logEvents.Select(x => x.ToString()).ToList());

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Initializes the required option.</summary>
        /// <remarks></remarks>
        /// =================================================================================================
        private void InitRequired()
        {
            _registerUrl = new Uri(new Uri(_remoteMLogOptions.ServiceClientAddress), _remoteMLogOptions.ServiceEventRegisterPath);

#if NET45
            _client = new HttpClient(new WebRequestHandler()
            {
                ClientCertificates = { _remoteMLogOptions.ServiceCertificate },
            });
            _client.Timeout = TimeSpan.FromMinutes(double.Parse(_remoteMLogOptions.ServiceTimeoutInMinute));
#endif
#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
            _client = new HttpClient(new HttpClientHandler { ClientCertificates = { _remoteMLogOptions.ServiceCertificate } });
            _client.Timeout = TimeSpan.FromMinutes(double.Parse(_remoteMLogOptions.ServiceTimeoutInMinute));
#endif
        }
    }
}