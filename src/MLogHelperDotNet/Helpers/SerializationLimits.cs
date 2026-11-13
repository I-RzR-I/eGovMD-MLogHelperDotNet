// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="SerializationLimits.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MLogHelperDotNet.Helpers
{
    /// <summary>
    ///     The limits that bound property value serialization.
    /// </summary>
    internal static class SerializationLimits
    {
        /// <summary>
        ///     (Immutable) The maximum depth of a serialized value tree.
        /// </summary>
        internal const int MaxDepth = 64;

        /// <summary>
        ///     (Immutable) The maximum number of nodes serialized for one property.
        /// </summary>
        internal const int MaxNodes = 10000;

        /// <summary>
        ///     (Immutable) The maximum number of value-type collection elements visited by the safety
        ///     walk.
        /// </summary>
        internal const int MaxWalkVisits = 100000;

        /// <summary>
        ///     (Immutable) The maximum number of characters written by the structural fallback for one
        ///     property.
        /// </summary>
        internal const int MaxFallbackChars = 1048576;

        /// <summary>
        ///     (Immutable) The maximum depth of a token tree.
        /// </summary>
        internal const int MaxTokenDepth = 128;

        /// <summary>
        ///     (Immutable) The maximum number of nodes of a token tree.
        /// </summary>
        internal const int MaxTokenNodes = 100000;
    }
}