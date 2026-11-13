// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="NewtonsoftTokenHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using MLogHelperDotNet.Enums;
using MLogHelperDotNet.Models;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using System;
using System.Collections;
using System.Collections.Concurrent;

#endregion

namespace MLogHelperDotNet.Helpers
{
    /// <summary>
    ///     Detects and writes Newtonsoft JSON tokens through reflection, within bounded limits.
    /// </summary>
    internal static class NewtonsoftTokenHelper
    {
        /// <summary>
        ///     (Immutable) The full name of the Newtonsoft JToken base type.
        /// </summary>
        private const string TokenTypeName = "Newtonsoft.Json.Linq.JToken";

        /// <summary>
        ///     (Immutable) The simple name of the Newtonsoft assembly.
        /// </summary>
        private const string TokenAssemblyName = "Newtonsoft.Json";

        /// <summary>
        ///     (Immutable) The token kinds that are not written through the compact form.
        /// </summary>
        private static readonly string[] RejectedTokenKinds =
        {
            "Raw", "Constructor", "Undefined", "Comment"
        };

        /// <summary>
        ///     (Immutable) The cache of the reflection accessors per JToken type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, TokenAccessors> TokenAccessorCache 
            = new ConcurrentDictionary<Type, TokenAccessors>();

        /// <summary>
        ///     Finds the Newtonsoft JToken type in the base chain of a type, matching by name and
        ///     assembly only.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <returns>
        ///     The JToken type, or null when the type is not a JToken.
        /// </returns>
        internal static Type FindNewtonsoftTokenBase(Type type)
        {
            for (var current = type; current.IsNotNull(); current = current.BaseType)
            {
                if (current!.FullName == TokenTypeName && current.Assembly.GetName().Name == TokenAssemblyName)
                    return current;
            }

            return null;
        }

        /// <summary>
        ///     Serializes a Newtonsoft token to compact JSON through reflection, after bounding the
        ///     whole token tree.
        /// </summary>
        /// <param name="token">The token instance.</param>
        /// <returns>
        ///     The JSON text, the escaped token text, or null text when the token exceeds the limits.
        /// </returns>
        internal static string TokenToJson(object token)
        {
            TokenCheck? check = null;
            try
            {
                var accessors = TokenAccessorCache.GetOrAdd(FindNewtonsoftTokenBase(token.GetType()), TokenAccessors.Create);
                if (accessors.IsNull())
                    return "null";

                var budget = SerializationLimits.MaxTokenNodes;
                check = CheckTokenTree(token, accessors, 0, ref budget);
                if (check == TokenCheck.LimitExceeded)
                    return "null";

                if (check == TokenCheck.Safe && accessors.TokenType.GetValue(token, null)?.ToString() != "Property")
                {
                    var json = accessors.ToJson.Invoke(token, new object[]
                    {
                        accessors.FormattingNone, accessors.NoConverters
                    }) as string;
                    if (json.IsNotNull() && json!.IndexOf('\n') < 0 && json.IndexOf('\r') < 0)
                        return json;
                }

                return BoundedTokenText(token);
            }
            catch (Exception)
            {
                return check == TokenCheck.Safe || check == TokenCheck.RejectedKind 
                    ? BoundedTokenText(token) 
                    : "null";
            }
        }

        /// <summary>
        ///     Measures a token tree and reports whether it is within the limits and made only of well-
        ///     formed kinds.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="accessors">The reflection accessors.</param>
        /// <param name="depth">The current token depth.</param>
        /// <param name="budget">[in,out] The remaining token budget.</param>
        /// <returns>
        ///     LimitExceeded as soon as a limit is hit, RejectedKind when a rejected kind exists within
        ///     the limits, otherwise Safe.
        /// </returns>
        private static TokenCheck CheckTokenTree(object token, TokenAccessors accessors, int depth, ref int budget)
        {
            if (depth > SerializationLimits.MaxTokenDepth || --budget < 0)
                return TokenCheck.LimitExceeded;

            var kind = accessors.TokenType.GetValue(token, null)?.ToString();
            var result = kind.IsNull() || Array.IndexOf(RejectedTokenKinds, kind) >= 0 ? TokenCheck.RejectedKind : TokenCheck.Safe;

            foreach (var child in (IEnumerable)token)
            {
                var childResult = CheckTokenTree(child, accessors, depth + 1, ref budget);
                if (childResult == TokenCheck.LimitExceeded)
                    return TokenCheck.LimitExceeded;
                if (childResult == TokenCheck.RejectedKind)
                    result = TokenCheck.RejectedKind;
            }

            return result;
        }

        /// <summary>
        ///     Writes the text of a token whose tree is already known to be bounded, as an escaped JSON
        ///     string.
        /// </summary>
        /// <param name="token">The bounded token.</param>
        /// <returns>
        ///     The JSON text, or null text when the token text cannot be produced.
        /// </returns>
        private static string BoundedTokenText(object token)
        {
            string text;
            try
            {
                text = token.ToString();
            }
            catch (Exception)
            {
                text = null;
            }

            return text.IsMissing()
                ? "null" 
                : JsonHelper.EscapeObject(text);
        }
    }
}