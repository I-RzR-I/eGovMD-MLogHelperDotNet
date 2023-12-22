// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:16
// ***********************************************************************
//  <copyright file="MNotifyPostConfigureOptions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.Extensions.Options;
using MLogHelperDotNet.Extensions;
using MLogHelperDotNet.Models;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

// ReSharper disable CollectionNeverUpdated.Local

#endregion

namespace MLogHelperDotNet.Configurations.Options
{
    /// <summary>
    ///     MLog POST configuration options
    /// </summary>
    public class MLogPostConfigureOptions : IPostConfigureOptions<RemoteMLogOptions>
    {
        /// <inheritdoc />
        public void PostConfigure(string name, RemoteMLogOptions mLogOptions)
        {
            if (mLogOptions.ServiceClientAddress.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"Please provide a {nameof(mLogOptions.ServiceClientAddress)}");
            }

            if (mLogOptions.ServiceCertificatePath.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"Please provide a {nameof(mLogOptions.ServiceCertificatePath)}");
            }

            if (mLogOptions.ServiceCertificatePassword.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"Please provide a {nameof(mLogOptions.ServiceCertificatePassword)}");
            }

            if (mLogOptions.ServiceEventRegisterPath.IsNullOrEmpty())
            {
                mLogOptions.ServiceEventRegisterPath = "register";
            }

            if (mLogOptions.ServiceEventGetPath.IsNullOrEmpty())
            {
                mLogOptions.ServiceEventGetPath = "query";
            }

            var certificate = new X509Certificate2Collection(CertificateLoader.Private(
                mLogOptions.ServiceCertificatePath,
                mLogOptions.ServiceCertificatePassword));

            if (certificate.Count.IsZero())
            {
                throw new ApplicationException("Invalid service certificate path or password");
            }

            if (certificate.Count.IsGreaterThanZero())
            {
                mLogOptions.ServiceCertificate = certificate[0];
            }

            mLogOptions.BasicHttpsBinding = new BasicHttpsBinding
            {
                Security =
                {
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Certificate
                    },
                    Mode = BasicHttpsSecurityMode.Transport
                },
                MaxReceivedMessageSize = 2147483647,
                CloseTimeout = TimeSpan.FromMinutes(20)
            };

            mLogOptions.BasicHttpBinding = new BasicHttpBinding
            {
                Security =
                {
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Certificate
                    },
                    Mode = BasicHttpSecurityMode.Transport
                },
                MaxReceivedMessageSize = 2147483647,
                CloseTimeout = TimeSpan.FromMinutes(20)
            };

            mLogOptions.EndpointAddress = new EndpointAddress(mLogOptions.ServiceClientAddress);
        }
    }
}
#endif