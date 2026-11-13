// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="SafePathWalkerHelper.cs" company="">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace MLogHelperDotNet.Helpers
{
    /// <summary>
    ///     Decides, without calling property getters, whether a value must avoid the data contract
    ///     serializer.
    /// </summary>
    internal static class SafePathWalkerHelper
    {
        /// <summary>
        ///     (Immutable) The cache of the types that can reach a token or are recursive collections.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> UnsafeTypeCache = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        ///     (Immutable) The cache of the types whose instances are walked by the safety check.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> WalkableTypeCache = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        ///     (Immutable) The cache of the Key and Value accessors of the constructed KeyValuePair
        ///     types.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> KeyValuePairCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        ///     Determines whether a type can reach a JToken or is a recursive collection, with a cached
        ///     result.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     True when the type must not be handed to the data contract serializer.
        /// </returns>
        private static bool TypeIsUnsafe(Type type)
        {
            return UnsafeTypeCache.GetOrAdd(type, t => TypeGraphIsUnsafe(t, new HashSet<Type>()));
        }

        /// <summary>
        ///     Walks the type graph (base chain, array element, generic arguments) looking for unsafe
        ///     types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="visited">The visited types.</param>
        /// <returns>
        ///     True when an unsafe type is reachable.
        /// </returns>
        private static bool TypeGraphIsUnsafe(Type type, HashSet<Type> visited)
        {
            if (type.IsNull() || !visited.Add(type))
                return false;
            if (NewtonsoftTokenHelper.FindNewtonsoftTokenBase(type).IsNotNull() || IsRecursiveCollection(type))
                return true;
            if (type.IsArray && TypeGraphIsUnsafe(type.GetElementType(), visited))
                return true;

            for (var current = type; current.IsNotNull(); current = current.BaseType)
            {
                if (current!.IsGenericType)
                {
                    foreach (var argument in current.GetGenericArguments())
                    {
                        if (TypeGraphIsUnsafe(argument, visited))
                            return true;
                    }
                }
            }

            foreach (var contract in type.GetInterfaces())
            {
                if (contract.IsGenericType)
                {
                    foreach (var argument in contract.GetGenericArguments())
                    {
                        if (TypeGraphIsUnsafe(argument, visited))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Determines whether a type is an enumerable of itself, such as a class deriving from List
        ///     of itself.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     True when the type implements IEnumerable of the type itself.
        /// </returns>
        private static bool IsRecursiveCollection(Type type)
        {
            foreach (var contract in type.GetInterfaces())
            {
                if (!contract.IsGenericType || contract.GetGenericTypeDefinition() != typeof(IEnumerable<>)) 
                    continue;
                if (contract.GetGenericArguments()[0] == type) 
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines whether a type is a simple leaf type, or a nullable of one, that cannot hold a
        ///     token.
        /// </summary>
        /// <param name="type">The element type.</param>
        /// <returns>
        ///     True for primitives, enums, string, decimal, DateTime, DateTimeOffset, TimeSpan and Guid.
        /// </returns>
        private static bool IsLeafElementType(Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type) ?? type;

            return underlying.IsPrimitive || underlying.IsEnum || underlying == typeof(string) || underlying == typeof(decimal)
                   || underlying == typeof(DateTime) || underlying == typeof(DateTimeOffset) || underlying == typeof(TimeSpan)
                   || underlying == typeof(Guid);
        }

        /// <summary>
        ///     Determines whether a type is a collection whose instances can be walked without calling
        ///     property getters.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     True for ICollection, ICollection of T and IReadOnlyCollection of T implementations,
        ///     except non-IDictionary types whose every closed collection and enumerable interface has a
        ///     leaf element type.
        /// </returns>
        private static bool IsWalkableCollection(Type type)
        {
            return WalkableTypeCache.GetOrAdd(type, t =>
            {
                if (t.IsArray)
                    return !IsLeafElementType(t.GetElementType());
                if (typeof(IDictionary).IsAssignableFrom(t))
                    return true;

                var isCollection = typeof(ICollection).IsAssignableFrom(t);
                var typed = false;
                var allLeaf = true;
                foreach (var contract in t.GetInterfaces())
                {
                    if (!contract.IsGenericType) continue;
                    var definition = contract.GetGenericTypeDefinition();
                    var isTypedCollection = definition == typeof(ICollection<>) || definition == typeof(IReadOnlyCollection<>);
                    if (!isTypedCollection && definition != typeof(IEnumerable<>)) continue;

                    typed = true;
                    if (isTypedCollection) isCollection = true;
                    if (!IsLeafElementType(contract.GetGenericArguments()[0])) allLeaf = false;
                }

                return isCollection && !(typed && allLeaf);
            });
        }

        /// <summary>
        ///     Reads the Key and Value of a boxed KeyValuePair through its BCL getters.
        /// </summary>
        /// <param name="item">The candidate element.</param>
        /// <param name="key">[out] The pair key.</param>
        /// <param name="value">[out] The pair value.</param>
        /// <returns>
        ///     True when the element is a constructed KeyValuePair.
        /// </returns>
        internal static bool TryReadKeyValuePair(object item, out object key, out object value)
        {
            key = null;
            value = null;
            var type = item?.GetType();
            if (type.IsNull() || !type!.IsGenericType || type.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
                return false;

            var accessors = KeyValuePairCache.GetOrAdd(type, t => new[]
            {
                t.GetProperty("Key"), t.GetProperty("Value")
            });
            key = accessors[0].GetValue(item, null);
            value = accessors[1].GetValue(item, null);

            return true;
        }

        /// <summary>
        ///     Checks, without calling property getters, whether a value or its collection elements are
        ///     unsafe for the data contract serializer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <returns>
        ///     True when the structural path must be used. False when nothing unsafe was found,
        ///     including when the walk was cut short or threw.
        /// </returns>
        internal static bool InstanceNeedsSafePath(object value, int depth, SerializationContext context)
        {
            if (value.IsNull())
                return false;

            try
            {
                return WalkValue(value, depth, context);
            }
            catch (Exception)
            {
                context.WalkFailures++;

                return false;
            }
        }

        /// <summary>
        ///     Walks one value. An unsafe result is memoised, a safe result only when the walk of its
        ///     subtree was complete.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <returns>
        ///     True when the structural path must be used.
        /// </returns>
        private static bool WalkValue(object value, int depth, SerializationContext context)
        {
            var type = value.GetType();

            if (TypeIsUnsafe(type))
                return true;
            if (!IsWalkableCollection(type))
                return false;

            if (depth >= SerializationLimits.MaxDepth)
            {
                context.WalkCuts++;
                context.WalkDepthCuts++;
                return false;
            }

            var isReference = !type.IsValueType;
            if (isReference)
            {
                if (context.SafePathMemo.TryGetValue(value, out var known))
                    return known;
                if (context.WalkCompleted.TryGetValue(value, out var exploredAt) && exploredAt <= depth)
                {
                    context.WalkCuts++;
                    return false;
                }

                if (!context.WalkInProgress.Add(value))
                {
                    context.WalkCuts++;
                    return false;
                }
            }

            var cutsBefore = context.WalkCuts;
            var failuresBefore = context.WalkFailures;
            try
            {
                var found = ElementsNeedSafePath(value, depth, context);
                if (isReference)
                {
                    if (found)
                        context.SafePathMemo[value] = true;
                    else if (context.WalkFailures == failuresBefore && context.WalkCuts == cutsBefore)
                        context.SafePathMemo[value] = false;
                    else
                        context.WalkCompleted[value] = depth;
                }

                return found;
            }
            finally
            {
                if (isReference) context.WalkInProgress.Remove(value);
            }
        }

        /// <summary>
        ///     Walks the elements of a collection for the safety check.
        /// </summary>
        /// <param name="value">The collection.</param>
        /// <param name="depth">The depth of the collection.</param>
        /// <param name="context">The per-property serialization context.</param>
        /// <returns>
        ///     True when an element needs the structural path.
        /// </returns>
        private static bool ElementsNeedSafePath(object value, int depth, SerializationContext context)
        {
            var countsElements = value.GetType().IsValueType;
            if (value is IDictionary dictionary)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (countsElements && ++context.WalkVisits > SerializationLimits.MaxWalkVisits) return StopWalk(context);
                    if (InstanceNeedsSafePath(entry.Key, depth + 1, context) || InstanceNeedsSafePath(entry.Value, depth + 1, context))
                        return true;
                }

                return false;
            }

            foreach (var item in (IEnumerable)value)
            {
                if (countsElements && ++context.WalkVisits > SerializationLimits.MaxWalkVisits) return StopWalk(context);
                if (item.IsNotNull() && TryReadKeyValuePair(item, out var key, out var itemValue))
                {
                    if (TypeIsUnsafe(item.GetType())
                        || InstanceNeedsSafePath(key, depth + 1, context)
                        || InstanceNeedsSafePath(itemValue, depth + 1, context))
                        return true;
                }
                else if (InstanceNeedsSafePath(item, depth + 1, context))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Ends a walk whose value-type element budget is spent and marks the walk as failed so
        ///     nothing is memoised.
        /// </summary>
        /// <param name="context">The per-property serialization context.</param>
        /// <returns>
        ///     Always false, so the value keeps the data contract serializer path.
        /// </returns>
        private static bool StopWalk(SerializationContext context)
        {
            context.WalkFailures++;

            return false;
        }
    }
}