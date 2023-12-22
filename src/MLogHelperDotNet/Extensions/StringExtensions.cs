// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-02 00:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-02 01:10
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace MLogHelperDotNet.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A string extensions.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    internal static class StringExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that query if 'source' is null or white space.
        /// </summary>
        /// <remarks>RzR, 02-Dec-23.</remarks>
        /// <param name="source">The source to act on.</param>
        /// <returns>True if null or white space, false if not.</returns>
        /// =================================================================================================
        internal static bool IsNullOrWhiteSpace(this string source)
            => source.IsNullOrEmpty() || string.IsNullOrWhiteSpace(source);
    }
}