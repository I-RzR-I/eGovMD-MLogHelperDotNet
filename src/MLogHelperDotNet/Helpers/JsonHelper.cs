// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-18 23:23
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-18 23:30
// ***********************************************************************
//  <copyright file="JsonHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System.Text;

#endregion

namespace MLogHelperDotNet.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A JSON helper.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    internal static class JsonHelper
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Escape object.</summary>
        /// <remarks></remarks>
        /// <param name="obj">The object.</param>
        /// <returns>A string.</returns>
        ///=================================================================================================
        internal static string EscapeObject(object obj)
        {
            var result = new StringBuilder();
            AppendEscapedJsonObject(result, obj);

            return result.ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Appends an escaped JSON object.</summary>
        /// <remarks></remarks>
        /// <param name="result">The result.</param>
        /// <param name="obj">The object.</param>
        ///=================================================================================================
        internal static void AppendEscapedJsonObject(StringBuilder result, object obj)
        {
            if (obj.IsNull())
            {
                result.Append("null");
            }
            else
            {
                var objectValue = obj.ToString();
                result.Append('"');
                var length = objectValue.Length;
                for (var index = 0; index < length; ++index)
                {
                    var charValue = objectValue[index];
                    switch (charValue)
                    {
                        case '\b':
                            result.Append("\\b");
                            break;
                        case '\t':
                            result.Append("\\t");
                            break;
                        case '\n':
                            result.Append("\\n");
                            break;
                        case '\f':
                            result.Append("\\f");
                            break;
                        case '\r':
                            result.Append("\\r");
                            break;
                        case '"':
                        case '/':
                        case '\\':
                            result.Append('\\');
                            result.Append(charValue);
                            break;
                        default:
                            if (charValue < ' ')
                            {
                                result.Append("\\u" + ((int)charValue).ToString("x4"));
                                break;
                            }

                            result.Append(charValue);
                            break;
                    }
                }

                result.Append('"');
            }
        }
    }
}