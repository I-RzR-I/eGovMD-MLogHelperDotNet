// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:19
// ***********************************************************************
//  <copyright file="MLogPostConfigureOptionsNetFramework.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using DomainCommonExtensions.DataTypeExtensions;
using MLogHelperDotNet.Configurations;
using MLogHelperDotNet.Extensions;
using MLogHelperDotNet.Helpers;
using MLogHelperDotNet.Models;
// ReSharper disable CollectionNeverUpdated.Local

#endregion

namespace MNotifyHelperDotNet.Configurations.Options
{
    /// <summary>
    ///     Post sign configuration
    /// </summary>
    /// <remarks></remarks>
    internal static class MLogPostConfigureOptionsNetFramework
    {
        internal static RemoteMLogOptions InitOptions()
        {
            var serviceConfigurationOptions = MLogConfigurationSectionNetFramework.GetRemoteMLogOptions();

            var remoteServiceClientAddress = serviceConfigurationOptions.ServiceClientAddress;
            if (remoteServiceClientAddress.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceClientAddress)}");
            }

            var serviceCertificatePath = serviceConfigurationOptions.ServiceCertificatePath;
            if (serviceCertificatePath.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceCertificatePath)}");
            }

            var serviceCertificatePassword = serviceConfigurationOptions.ServiceCertificatePassword;
            if (serviceCertificatePassword.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceCertificatePassword)}");
            }

            var serviceEventRegisterPath = serviceConfigurationOptions.ServiceEventRegisterPath;
            if (serviceEventRegisterPath.IsNullOrEmpty())
            {
                serviceConfigurationOptions.ServiceEventRegisterPath = "register";
            }

            var serviceEventGetPath = serviceConfigurationOptions.ServiceEventGetPath;
            if (serviceEventGetPath.IsNullOrEmpty())
            {
                serviceConfigurationOptions.ServiceEventGetPath = "query";
            }

            var mLogOptions = new RemoteMLogOptions
            {
                ServiceEventRegisterPath = serviceConfigurationOptions.ServiceEventRegisterPath,
                ServiceEventGetPath = serviceConfigurationOptions.ServiceEventGetPath,
                ServiceClientAddress = remoteServiceClientAddress,
                ServiceCertificatePath = serviceCertificatePath,
                ServiceCertificatePassword = serviceCertificatePassword,
                BasicHttpBinding = new BasicHttpBinding
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
                    CloseTimeout = TimeSpan.FromMinutes(5)
                },
                BasicHttpsBinding = new BasicHttpsBinding
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
                    CloseTimeout = TimeSpan.FromMinutes(5)
                }
            };

            if (!serviceConfigurationOptions.ServiceTimeoutInMinute.IsNullOrWhiteSpace())
                mLogOptions.ServiceTimeoutInMinute = serviceConfigurationOptions.ServiceTimeoutInMinute;

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

            mLogOptions.EndpointAddress = new EndpointAddress(mLogOptions.ServiceClientAddress);

            return mLogOptions;
        }
    }
}

#endif