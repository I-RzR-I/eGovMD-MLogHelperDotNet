// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-08 00:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-19 22:02
// ***********************************************************************
//  <copyright file="MLogSoapClientService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using MLogHelperDotNet.Abstractions;
using MLogHelperDotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

#endregion

namespace MLogHelperDotNet.Services
{
    /// <inheritdoc cref="IMLogEndpointClientService" />
    public class MLogSoapClientService : IMLogEndpointClientService
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Default constructor.</summary>
        /// <remarks></remarks>
        /// =================================================================================================
        public MLogSoapClientService() 
            => throw new NotImplementedException("SOAP implementation currently is not available!");

        /// <inheritdoc />
        public IResult<string> RegisterEvent(string jsonEvent)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public IResult<string> RegisterEventBatch(IReadOnlyCollection<string> jsonEvents)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventAsync(string jsonEvent)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventBatchAsync(IReadOnlyCollection<string> jsonEvents)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public IResult<string> RegisterEvent(MLogEventDto logEvent)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public IResult<string> RegisterEventBatch(IEnumerable<MLogEventDto> logEvents)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventAsync(MLogEventDto logEvent)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task<IResult<string>> RegisterEventBatchAsync(IEnumerable<MLogEventDto> logEvents)
            => throw new NotImplementedException();
    }
}