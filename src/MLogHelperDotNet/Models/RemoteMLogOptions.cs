// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 08:23
// ***********************************************************************
//  <copyright file="RemoteMNotifyOptions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace MLogHelperDotNet.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Remote MLog option.</summary>
    /// <remarks></remarks>
    ///=================================================================================================
    public class RemoteMLogOptions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets link/address to remote service.</summary>
        /// <value>The service client address.</value>
        ///=================================================================================================
        public string ServiceClientAddress { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets path to event registration.</summary>
        /// <value>The full pathname of the service event register file.</value>
        ///=================================================================================================
        public string ServiceEventRegisterPath { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets path to event get/query.</summary>
        /// <value>The full pathname of the service event get file.</value>
        ///=================================================================================================
        public string ServiceEventGetPath { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Path of service certificate.</summary>
        /// <value>The full pathname of the service certificate file.</value>
        ///=================================================================================================
        public string ServiceCertificatePath { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Password for service certificate.</summary>
        /// <value>The service certificate password.</value>
        ///=================================================================================================
        public string ServiceCertificatePassword { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Timeout for service call.</summary>
        /// <value>The service certificate password.</value>
        ///=================================================================================================
        public string ServiceTimeoutInMinute { get; set; } = "5";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Service certificate.</summary>
        /// <value>The service certificate.</value>
        ///=================================================================================================
        public X509Certificate2 ServiceCertificate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets BasicHttpsBinding.</summary>
        /// <value>The basic HTTPS binding.</value>
        ///=================================================================================================
        public BasicHttpsBinding BasicHttpsBinding { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets BasicHttpBinding.</summary>
        /// <value>The basic HTTP binding.</value>
        ///=================================================================================================
        public BasicHttpBinding BasicHttpBinding { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets EndpointAddress.</summary>
        /// <value>The endpoint address.</value>
        ///=================================================================================================
        public EndpointAddress EndpointAddress { get; set; }
    }
}