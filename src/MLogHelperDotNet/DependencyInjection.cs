// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-01 09:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-03 21:31
// ***********************************************************************
//  <copyright file="DependencyInjection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

#region U S A G E S

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MLogHelperDotNet.Abstractions;
using MLogHelperDotNet.Configurations.Options;
using MLogHelperDotNet.Enums;
using MLogHelperDotNet.Extensions;
using MLogHelperDotNet.Models;
using MLogHelperDotNet.Services;
using System;

#endregion

namespace MLogHelperDotNet
{
    /// <summary>
    ///     MLog service DI
    /// </summary>
    /// <remarks></remarks>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Add MLog service
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IServiceCollection AddMLogService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddOptions<RemoteMLogOptions>();
            var mLogOptions = configuration.GetSection(nameof(RemoteMLogOptions)).Get<RemoteMLogOptions>();

            services.Configure<RemoteMLogOptions>(options =>
            {
                options.ServiceClientAddress = mLogOptions.ServiceClientAddress;
                options.ServiceEventRegisterPath = mLogOptions.ServiceEventRegisterPath;
                options.ServiceEventGetPath = mLogOptions.ServiceEventGetPath;
                options.ServiceCertificatePath = mLogOptions.ServiceCertificatePath;
                options.ServiceCertificatePassword = mLogOptions.ServiceCertificatePassword;
                if (!mLogOptions.ServiceTimeoutInMinute.IsNullOrWhiteSpace())
                    options.ServiceTimeoutInMinute = mLogOptions.ServiceTimeoutInMinute;
            });
            services.PostConfigure<RemoteMLogOptions>(options =>
            {
                options.ServiceClientAddress = mLogOptions.ServiceClientAddress;
                options.ServiceEventRegisterPath = mLogOptions.ServiceEventRegisterPath;
                options.ServiceEventGetPath = mLogOptions.ServiceEventGetPath;
                options.ServiceCertificatePath = mLogOptions.ServiceCertificatePath;
                options.ServiceCertificatePassword = mLogOptions.ServiceCertificatePassword;
                if (!mLogOptions.ServiceTimeoutInMinute.IsNullOrWhiteSpace())
                    options.ServiceTimeoutInMinute = mLogOptions.ServiceTimeoutInMinute;
            });
            services.AddSingleton(mLogOptions);

            services.AddSingleton<IPostConfigureOptions<RemoteMLogOptions>, MLogPostConfigureOptions>();

            services.AddSingleton<MLogRestClientService>();
            services.AddSingleton<MLogSoapClientService>();

            services.AddSingleton<Func<EndpointType, IMLogEndpointClientService>>(sp => endpointType =>
            {
                return endpointType switch
                {
                    EndpointType.REST => sp.GetRequiredService<MLogRestClientService>(),
                    EndpointType.SOAP => throw new NotImplementedException($"SOAP implementation currently is not available!"),
                    _ => throw new NotImplementedException($"No implementation for endpoint type: {endpointType}")
                };
            });

            return services;
        }
    }
}
#endif