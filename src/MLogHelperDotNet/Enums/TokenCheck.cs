// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="TokenCheck.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MLogHelperDotNet.Enums
{
    /// <summary>
    ///     The outcome of measuring a token tree.
    /// </summary>
    internal enum TokenCheck
    {
        /// <summary>An enum constant representing the safe option.</summary>
        Safe,

        /// <summary>An enum constant representing the rejected kind option.</summary>
        RejectedKind,

        /// <summary>An enum constant representing the limit exceeded option.</summary>
        LimitExceeded
    }
}