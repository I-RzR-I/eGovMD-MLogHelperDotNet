// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="ReferenceComparer.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

namespace MLogHelperDotNet.Models
{
    /// <summary>
    ///     Compares objects by reference identity.
    /// </summary>
    /// <seealso cref="T:System.Collections.Generic.IEqualityComparer{Object}"/>
    internal sealed class ReferenceComparer : IEqualityComparer<object>
    {
        /// <summary>(Immutable) the shared comparer instance.</summary>
        public static readonly ReferenceComparer Instance = new ReferenceComparer();

        /// <inheritdoc />
        bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);

        /// <inheritdoc />
        int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}