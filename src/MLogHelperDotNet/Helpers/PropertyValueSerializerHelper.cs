// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="PropertyValueSerializerHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using MLogHelperDotNet.Models;
using RzR.Extensions.Domain.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace MLogHelperDotNet.Helpers
{
    /// <summary>
    ///     Serializes MLog event property values to JSON text without ever throwing.
    /// </summary>
    internal static class PropertyValueSerializerHelper
    {
        /// <summary>
        ///     Serializes a property value to JSON text without ever throwing.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>
        ///     The JSON text, or null text when the value cannot be serialized within the limits.
        /// </returns>
        internal static string SerializePropertyValue(object value)
        {
            try
            {
                return SerializeValue(value, new SerializationContext(), 0);
            }
            catch (Exception)
            {
                return "null";
            }
        }

        /// <summary>
        ///     Serializes a single node, choosing between the data contract serializer and the
        ///     structural fallback.
        /// </summary>
        /// <exception cref="NodeBudgetExceededException">
        ///     Thrown when a Node Budget Exceeded error condition occurs.
        /// </exception>
        /// <param name="value">The node value.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <param name="depth">The current depth.</param>
        /// <returns>
        ///     The JSON text.
        /// </returns>
        private static string SerializeValue(object value, SerializationContext context, int depth)
        {
            if (value.IsNull())
                return CountNested(context, "null");
            if (++context.Nodes > SerializationLimits.MaxNodes)
                throw new NodeBudgetExceededException();
            if (depth > SerializationLimits.MaxDepth)
                return CountNested(context, "null");

            var type = value.GetType();
            if (!type.IsValueType && context.Ancestors.Count > 0 && context.Ancestors.Contains(value))
                return CountNested(context, "null");
            if (NewtonsoftTokenHelper.FindNewtonsoftTokenBase(type).IsNotNull())
                return CountNested(context, NewtonsoftTokenHelper.TokenToJson(value));

            context.BeginWalk();
            var divert = SafePathWalkerHelper.InstanceNeedsSafePath(value, 0, context);
            if (!divert && context.WalkFailures == context.WalkFailuresAtBegin && context.WalkDepthCuts == 0)
            {
                if (!type.IsValueType) context.SafePathMemo[value] = false;
                foreach (var visited in new List<object>(context.WalkCompleted.Keys))
                    context.SafePathMemo[visited] = false;
            }

            if (!divert)
            {
                try
                {
                    return depth == 0 
                        ? SerializeWithDcjs(value) 
                        : SerializeWithDcjsCapped(value, context);
                }
                catch (Exception ex) when (ex is SerializationException || ex is InvalidDataContractException)
                {
                    if (context.FallbackChars > SerializationLimits.MaxFallbackChars) throw new NodeBudgetExceededException();

                    // The contract serializer rejected the shape, so the structural fallback takes over.
                }
            }

            return SerializeStructural(value, context, depth);
        }

        /// <summary>
        ///     Serializes a value with the data contract JSON serializer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The JSON text.
        /// </returns>
        private static string SerializeWithDcjs(object value)
        {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(value.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, value);

            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        ///     Serializes a nested value with the data contract JSON serializer, counting every byte
        ///     written against the output cap.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <returns>
        ///     The JSON text.
        /// </returns>
        private static string SerializeWithDcjsCapped(object value, SerializationContext context)
        {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(value.GetType());
            var ms = new CappedStream(context);
            serializer.WriteObject(ms, value);

            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        ///     Serializes an enumerable node element by element, following the data contract dictionary
        ///     convention.
        /// </summary>
        /// <param name="value">The node value.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <param name="depth">The current depth.</param>
        /// <returns>
        ///     The JSON text, or null text for a non-enumerable value.
        /// </returns>
        private static string SerializeStructural(object value, SerializationContext context, int depth)
        {
            if (!(value is IEnumerable enumerable))
                return CountNested(context, "null");
            if (!context.Ancestors.Add(value))
                return CountNested(context, "null");

            try
            {
                var builder = new StringBuilder("[");
                CountChars(context, 1);
                var first = true;
                if (value is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        if (!first) AppendLiteral(builder, ",", context);
                        first = false;
                        AppendKeyValue(builder, entry.Key, entry.Value, context, depth);
                    }
                }
                else
                {
                    foreach (var item in enumerable)
                    {
                        if (!first) AppendLiteral(builder, ",", context);
                        first = false;
                        if (item.IsNotNull() && SafePathWalkerHelper.TryReadKeyValuePair(item, out var key, out var itemValue))
                            AppendKeyValue(builder, key, itemValue, context, depth);
                        else
                            builder.Append(SerializeValue(item, context, depth + 1));
                    }
                }

                CountChars(context, 1);

                return builder.Append(']').ToString();
            }
            finally
            {
                context.Ancestors.Remove(value);
            }
        }

        /// <summary>
        ///     Appends a key and value pair using the Key and Value member names.
        /// </summary>
        /// <param name="builder">The output builder.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <param name="depth">The depth of the owning collection.</param>
        private static void AppendKeyValue(StringBuilder builder, object key, object value, SerializationContext context, int depth)
        {
            AppendLiteral(builder, "{\"Key\":", context);
            builder.Append(SerializeValue(key, context, depth + 1));
            AppendLiteral(builder, ",\"Value\":", context);
            builder.Append(SerializeValue(value, context, depth + 1));
            AppendLiteral(builder, "}", context);
        }

        /// <summary>
        ///     Appends structural punctuation or a literal and counts it against the per-property output
        ///     cap.
        /// </summary>
        /// <param name="builder">The output builder.</param>
        /// <param name="text">The literal text.</param>
        /// <param name="context">The per-property serialization context.</param>
        private static void AppendLiteral(StringBuilder builder, string text, SerializationContext context)
        {
            CountChars(context, text.Length);
            builder.Append(text);
        }

        /// <summary>
        ///     Counts text that a non-structural path returns to a structural parent, so every final
        ///     character is counted once.
        /// </summary>
        /// <param name="context">The per-property serialization context.</param>
        /// <param name="text">The returned text.</param>
        /// <returns>
        ///     The same text.
        /// </returns>
        private static string CountNested(SerializationContext context, string text)
        {
            if (context.Ancestors.Count > 0)
                CountChars(context, text.Length);

            return text;
        }

        /// <summary>
        ///     Adds to the per-property output counter and enforces the cap.
        /// </summary>
        /// <exception cref="NodeBudgetExceededException">
        ///     Thrown when a Node Budget Exceeded error condition occurs.
        /// </exception>
        /// <param name="context">The per-property serialization context.</param>
        /// <param name="length">The number of characters.</param>
        internal static void CountChars(SerializationContext context, int length)
        {
            context.FallbackChars += length;

            if (context.FallbackChars > SerializationLimits.MaxFallbackChars)
                throw new NodeBudgetExceededException();
        }
    }
}