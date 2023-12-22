// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-18 22:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-18 22:13
// ***********************************************************************
//  <copyright file="MLogLevelType.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MLogHelperDotNet.Enums
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Values that represent log level types.</summary>
    /// <remarks></remarks>
    ///=================================================================================================
    public enum MLogLevelType
    {
        /// <summary>An enum constant representing the unknown option.</summary>
        Unknown = 0,

        /// <summary>An enum constant representing the fatal option.</summary>
        Fatal = 1,

        /// <summary>An enum constant representing the error option.</summary>
        Error = 2,

        /// <summary>An enum constant representing the warning option.</summary>
        Warning = 3,

        /// <summary>An enum constant representing the information option.</summary>
        Information = 4,

        /// <summary>An enum constant representing the debug option.</summary>
        Debug = 5,

        /// <summary>An enum constant representing the trace option.</summary>
        Trace = 6
    }
}