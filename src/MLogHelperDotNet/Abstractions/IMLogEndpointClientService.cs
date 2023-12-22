// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-07 00:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-13 15:13
// ***********************************************************************
//  <copyright file="IMLogEndpointClientService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using MLogHelperDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace MLogHelperDotNet.Abstractions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Interface for im log endpoint client service.</summary>
    /// <remarks></remarks>
    ///=================================================================================================
    public interface IMLogEndpointClientService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event described by logEvent.</summary>
        /// <param name="jsonEvent">The JSON event.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        ///=================================================================================================
        IResult<string> RegisterEvent(string jsonEvent);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event batch described by logEvents.</summary>
        /// <param name="jsonEvents">The JSON events.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        ///=================================================================================================
        IResult<string> RegisterEventBatch(IReadOnlyCollection<string> jsonEvents);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event asynchronous described by logEvent.</summary>
        /// <param name="jsonEvent">The JSON event.</param>
        /// <returns>The register event.</returns>
        ///=================================================================================================
        Task<IResult<string>> RegisterEventAsync(string jsonEvent);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event batch asynchronous described by logEvents.</summary>
        /// <param name="jsonEvents">The JSON events.</param>
        /// <returns>The register event batch.</returns>
        ///=================================================================================================
        Task<IResult<string>> RegisterEventBatchAsync(IReadOnlyCollection<string> jsonEvents);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event described by logEvent.</summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        ///=================================================================================================
        IResult<string> RegisterEvent(MLogEventDto logEvent);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event batch described by logEvents.</summary>
        /// <param name="logEvents">The log events.</param>
        /// <returns>An IResult&lt;string&gt;</returns>
        ///=================================================================================================
        IResult<string> RegisterEventBatch(IEnumerable<MLogEventDto> logEvents);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event asynchronous described by logEvent.</summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>The register event.</returns>
        ///=================================================================================================
        Task<IResult<string>> RegisterEventAsync(MLogEventDto logEvent);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Registers the event batch asynchronous described by logEvents.</summary>
        /// <param name="logEvents">The log events.</param>
        /// <returns>The register event batch.</returns>
        ///=================================================================================================
        Task<IResult<string>> RegisterEventBatchAsync(IEnumerable<MLogEventDto> logEvents);
    }
}