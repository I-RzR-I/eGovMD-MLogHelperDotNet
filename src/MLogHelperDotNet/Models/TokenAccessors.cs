// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="TokenAccessors.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Primitives;
using System;
using System.Reflection;

#endregion

namespace MLogHelperDotNet.Models
{
    /// <summary>
    ///     Reflection members resolved from the Newtonsoft assembly that loaded the token.
    /// </summary>
    internal sealed class TokenAccessors
    {
        /// <summary>
        ///     The compact JSON writing method of the JToken type.
        /// </summary>
        /// <value>
        ///     The compact JSON writing method.
        /// </value>
        public MethodInfo ToJson { get; private set; }

        /// <summary>
        ///     The Type property of the JToken type.
        /// </summary>
        /// <value>
        ///     The token kind property.
        /// </value>
        public PropertyInfo TokenType { get; private set; }

        /// <summary>
        ///     The Formatting.None value.
        /// </summary>
        /// <value>
        ///     The Formatting.None value.
        /// </value>
        public object FormattingNone { get; private set; }

        /// <summary>
        ///     An empty JsonConverter array.
        /// </summary>
        /// <value>
        ///     An empty converter array.
        /// </value>
        public Array NoConverters { get; private set; }

        /// <summary>Resolves the accessors from the JToken type's own assembly.</summary>
        /// <param name="jTokenType">The JToken type.</param>
        /// <returns>The accessors, or null when a member is missing.</returns>
        public static TokenAccessors Create(Type jTokenType)
        {
            var formatting = jTokenType.Assembly.GetType("Newtonsoft.Json.Formatting", false);
            var converter = jTokenType.Assembly.GetType("Newtonsoft.Json.JsonConverter", false);
            if (formatting.IsNull() || converter.IsNull())
                return null;

            var toJson = jTokenType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null,
                new[] { formatting, converter.MakeArrayType() }, null);
            var tokenType = jTokenType.GetProperty("Type", BindingFlags.Public | BindingFlags.Instance);
            
            if (toJson.IsNull() || toJson!.ReturnType != typeof(string) || tokenType.IsNull())
                return null;

            return new TokenAccessors
            {
                ToJson = toJson,
                TokenType = tokenType,
                FormattingNone = Enum.ToObject(formatting, 0),
                NoConverters = Array.CreateInstance(converter, 0)
            };
        }
    }
}