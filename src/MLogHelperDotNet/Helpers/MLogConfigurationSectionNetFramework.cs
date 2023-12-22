// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-13 22:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 22:52
// ***********************************************************************
//  <copyright file="MNotifyConfigurationSectionNetFramework.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System;
using System.Configuration;
using MLogHelperDotNet.Models;
using MLogHelperDotNet.Helpers.ConfSection;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace MLogHelperDotNet.Helpers
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>MLog configuration section NET Framework.</summary>
    /// <remarks>RzR, 02-Dec-23.</remarks>
    ///=================================================================================================
    internal static class MLogConfigurationSectionNetFramework
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get MLog configuration options.</summary>
        /// <remarks>RzR, 02-Dec-23.</remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <returns>The remote m log options.</returns>
        ///=================================================================================================
        internal static RemoteMLogOptions GetRemoteMLogOptions()
        {
            var configuration = new RemoteMLogOptions();
            var configurationOptions = ConfigurationManager.GetSection("MLogConfigurationSection") as MLogConfigurationSection;
            if (configurationOptions.IsNull() || configurationOptions?.ServiceOptions.Count == 0)
            {
                throw new ArgumentNullException($@"{nameof(configurationOptions)} is null or empty");
            }

            // ReSharper disable once PossibleNullReferenceException
            foreach (MLogOptionConfigurationElement element in configurationOptions?.ServiceOptions)
            {
                if (element.Key.IsNullOrEmpty())
                {
                    throw new ArgumentNullException($@"{nameof(element.Key)} is null");
                }

                if (element.Value.IsNullOrEmpty())
                {
                    throw new ArgumentNullException($@"{nameof(element.Value)} is null");
                }

                switch (element.Key)
                {
                    case nameof(configuration.ServiceClientAddress):
                        configuration.ServiceClientAddress = element.Value;
                        break;
                    case nameof(configuration.ServiceEventRegisterPath):
                        configuration.ServiceEventRegisterPath = element.Value;
                        break;
                    case nameof(configuration.ServiceEventGetPath):
                        configuration.ServiceEventGetPath = element.Value;
                        break;
                    case nameof(configuration.ServiceCertificatePath):
                        configuration.ServiceCertificatePath = element.Value;
                        break;
                    case nameof(configuration.ServiceCertificatePassword):
                        configuration.ServiceCertificatePassword = element.Value;
                        break;
                    case nameof(configuration.ServiceTimeoutInMinute):
                        if (!string.IsNullOrEmpty(element.Value))
                            configuration.ServiceTimeoutInMinute = element.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(@$"{nameof(element.Key)} is out of range");
                }
            }

            return configuration;
        }
    }
}

#endif