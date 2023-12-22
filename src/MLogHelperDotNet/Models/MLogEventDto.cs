// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-12-18 22:22
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-18 23:35
// ***********************************************************************
//  <copyright file="MLogEventDto.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using MLogHelperDotNet.Extensions;
using MLogHelperDotNet.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable RedundantDictionaryContainsKeyBeforeAdding
// ReSharper disable InconsistentNaming

#endregion

namespace MLogHelperDotNet.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A log event data transfer object.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    [Serializable]
    [DataContract]
    public class MLogEventDto
    {
        /// <summary>(Immutable) the properties.</summary>
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks></remarks>
        /// =================================================================================================
        public MLogEventDto() { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks></remarks>
        /// <param name="eventType">The type of the event.</param>
        /// =================================================================================================
        public MLogEventDto(string eventType)
        {
            EventTime = DateTime.Now;
            EventType = eventType;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the event time.</summary>
        /// <value>The event time.</value>
        /// =================================================================================================
        [DataMember]
        public DateTime EventTime
        {
            get => GetProperty("event_time") as DateTime? ?? DateTime.Now;
            set => SetProperty("event_time", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the type of the event.</summary>
        /// <value>The type of the event.</value>
        /// =================================================================================================
        [DataMember]
        public string EventType
        {
            get => GetProperty("event_type")?.ToString();
            set => SetProperty("event_type", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the identifier of the event.</summary>
        /// <value>The identifier of the event.</value>
        /// =================================================================================================
        [DataMember]
        public string EventID
        {
            get => GetProperty("event_id")?.ToString();
            set => SetProperty("event_id", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the event correlation.</summary>
        /// <value>The event correlation.</value>
        /// =================================================================================================
        [DataMember]
        public string EventCorrelation
        {
            get => GetProperty("event_correlation")?.ToString();
            set => SetProperty("event_correlation", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the event level.</summary>
        /// <value>The event level.</value>
        /// =================================================================================================
        [DataMember]
        public string EventLevel
        {
            get => GetProperty("event_level")?.ToString();
            set => SetProperty("event_level", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the event source.</summary>
        /// <value>The event source.</value>
        /// =================================================================================================
        [DataMember]
        public string EventSource
        {
            get => GetProperty("event_source")?.ToString();
            set => SetProperty("event_source", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets a message describing the event.</summary>
        /// <value>A message describing the event.</value>
        /// =================================================================================================
        [DataMember]
        public string EventMessage
        {
            get => GetProperty("event_message")?.ToString();
            set => SetProperty("event_message", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the event details.</summary>
        /// <value>The event details.</value>
        /// =================================================================================================
        [DataMember]
        public object EventDetails
        {
            get => GetProperty("event_details");
            set => SetProperty("event_details", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the legal entity.</summary>
        /// <value>The legal entity.</value>
        /// =================================================================================================
        [DataMember]
        public string LegalEntity
        {
            get => GetProperty("legal_entity")?.ToString();
            set => SetProperty("legal_entity", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the legal basis.</summary>
        /// <value>The legal basis.</value>
        /// =================================================================================================
        [DataMember]
        public object LegalBasis
        {
            get => GetProperty("legal_basis");
            set => SetProperty("legal_basis", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the legal reason.</summary>
        /// <value>The legal reason.</value>
        /// =================================================================================================
        [DataMember]
        public object LegalReason
        {
            get => GetProperty("legal_reason");
            set => SetProperty("legal_reason", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the user.</summary>
        /// <value>The user.</value>
        /// =================================================================================================
        [DataMember]
        public string User
        {
            get => GetProperty("user")?.ToString();
            set => SetProperty("user", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the user session.</summary>
        /// <value>The user session.</value>
        /// =================================================================================================
        [DataMember]
        public string UserSession
        {
            get => GetProperty("user_session")?.ToString();
            set => SetProperty("user_session", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the user address.</summary>
        /// <value>The user address.</value>
        /// =================================================================================================
        [DataMember]
        public string UserAddress
        {
            get => GetProperty("user_address")?.ToString();
            set => SetProperty("user_address", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the subject.</summary>
        /// <value>The subject.</value>
        /// =================================================================================================
        [DataMember]
        public object Subject
        {
            get => GetProperty("subject");
            set => SetProperty("subject", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the type of the subject.</summary>
        /// <value>The type of the subject.</value>
        /// =================================================================================================
        [DataMember]
        public object SubjectType
        {
            get => GetProperty("subject_type");
            set => SetProperty("subject_type", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the name of the subject.</summary>
        /// <value>The name of the subject.</value>
        /// =================================================================================================
        [DataMember]
        public string SubjectName
        {
            get => GetProperty("subject_name")?.ToString();
            set => SetProperty("subject_name", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the object.</summary>
        /// <value>The object.</value>
        /// =================================================================================================
        [DataMember]
        public object Object
        {
            get => GetProperty("object");
            set => SetProperty("object", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the type of the object.</summary>
        /// <value>The type of the object.</value>
        /// =================================================================================================
        [DataMember]
        public string ObjectType
        {
            get => GetProperty("object_type")?.ToString();
            set => SetProperty("object_type", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the name of the object.</summary>
        /// <value>The name of the object.</value>
        /// =================================================================================================
        [DataMember]
        public string ObjectName
        {
            get => GetProperty("object_name")?.ToString();
            set => SetProperty("object_name", value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Sets a property.</summary>
        /// <remarks></remarks>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        private void SetProperty(string key, object value)
        {
            if (value.IsNull() || (value is string str && str.IsNullOrWhiteSpace()) || value is ICollection { Count: 0 })
                properties.Remove(key);
            else if (properties.ContainsKey(key))
                properties[key] = value;
            else
                properties.Add(key, value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets a property.</summary>
        /// <remarks></remarks>
        /// <param name="key">The key.</param>
        /// <returns>The property.</returns>
        /// =================================================================================================
        private object GetProperty(string key)
        {
            properties.TryGetValue(key, out var property);

            return property;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = new StringBuilder("{", properties.Count * 32);
            foreach (var property in properties)
            {
                if (property.Value.IsNotNull())
                {
                    if (result.Length > 1)
                        result.Append(',');
                    JsonHelper.AppendEscapedJsonObject(result, property.Key);
                    result.Append(":");
                    
                    if (property.Value.GetType().IsTypeOfDateTime())
                    {
                        var dateTime = (DateTime?)property.Value;

                        result.Append('"');
                        result.Append(dateTime?.ToString("O"));
                        result.Append('"');
                    }
                    else
                    {
                        var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(property.Value.GetType());
                        var ms = new MemoryStream();
                        serializer.WriteObject(ms, property.Value);
                        var json = Encoding.Default.GetString(ms.ToArray());

                        result.Append(json);
                    }
                }
            }

            return result.Append('}').ToString();
        }
    }
}