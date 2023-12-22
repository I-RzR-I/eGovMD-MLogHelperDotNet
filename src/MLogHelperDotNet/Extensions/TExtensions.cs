// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-01 09:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-02 01:11
// ***********************************************************************
//  <copyright file="TExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;

#endregion

// ReSharper disable InconsistentNaming

namespace MLogHelperDotNet.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>T extensions.</summary>
    /// <remarks>RzR, 02-Dec-23.</remarks>
    /// =================================================================================================
    public static class TExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get sub array.</summary>
        /// <remarks>RzR, 02-Dec-23.</remarks>
        /// <typeparam name="T">Source type.</typeparam>
        /// <param name="array">Source array.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="length">Take.</param>
        /// <returns>A T[].</returns>
        /// =================================================================================================
        public static T[] SubArray<T>(this T[] array, int offset, int length) => array.Skip(offset)
            .Take(length)
            .ToArray();

        internal static bool IsTypeOfDateTime(this Type sourceType)
            => sourceType == typeof(DateTime) || sourceType == typeof(DateTime?);
    }
}