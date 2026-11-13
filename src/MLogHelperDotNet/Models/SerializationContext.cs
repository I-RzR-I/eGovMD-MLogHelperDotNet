// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="SerializationContext.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace MLogHelperDotNet.Models
{
    /// <summary>
    ///     Per-property state: counters, the safety memo and the ancestors of the node being
    ///     serialized.
    /// </summary>
    internal sealed class SerializationContext
    {
        /// <summary>
        ///     The number of nodes visited.
        /// </summary>
        public int Nodes;

        /// <summary>
        ///     The number of value-type collection elements visited by the safety walk.
        /// </summary>
        public int WalkVisits;

        /// <summary>
        ///     The number of walk cuts (depth cut, in-progress hit or completed skip).
        /// </summary>
        public int WalkCuts;

        /// <summary>
        ///     The number of walk failures (swallowed exception or spent budget).
        /// </summary>
        public int WalkFailures;

        /// <summary>
        ///     The failure count recorded when the current root walk began.
        /// </summary>
        public int WalkFailuresAtBegin;

        /// <summary>
        ///     The number of depth-cap cuts in the current root walk.
        /// </summary>
        public int WalkDepthCuts;

        /// <summary>
        ///     The number of output characters counted against the cap.
        /// </summary>
        public long FallbackChars;

        /// <summary>
        ///     (Immutable) the ancestors of the node being serialized.
        /// </summary>
        public readonly HashSet<object> Ancestors = new HashSet<object>(ReferenceComparer.Instance);

        /// <summary>
        ///     (Immutable) the memoised safety results per reference.
        /// </summary>
        public readonly Dictionary<object, bool> SafePathMemo = new Dictionary<object, bool>(ReferenceComparer.Instance);

        /// <summary>
        ///     (Immutable) the values whose walk is still running.
        /// </summary>
        public readonly HashSet<object> WalkInProgress = new HashSet<object>(ReferenceComparer.Instance);

        /// <summary>
        ///     (Immutable) the depth at which an incomplete walk of a value ended.
        /// </summary>
        public readonly Dictionary<object, int> WalkCompleted = new Dictionary<object, int>(ReferenceComparer.Instance);

        /// <summary>
        ///     Clears the state that is valid only within a single root walk.
        /// </summary>
        public void BeginWalk()
        {
            WalkInProgress.Clear();
            WalkCompleted.Clear();
            WalkDepthCuts = 0;
            WalkFailuresAtBegin = WalkFailures;
        }
    }
}