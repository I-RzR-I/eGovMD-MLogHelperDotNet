// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="NodeBudgetExceededException.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace MLogHelperDotNet.Models
{
    /// <summary>
    ///     Signals that a property exceeded the node budget or the fallback output cap.
    /// </summary>
    /// <seealso cref="T:Exception"/>
    internal sealed class NodeBudgetExceededException : Exception
    {
    }
}