// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-13 22:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 22:58
// ***********************************************************************
//  <copyright file="MLogConfigurationSection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System.Configuration;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MLogHelperDotNet.Helpers.ConfSection
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>MLog service configuration sections.</summary>
    /// <remarks>RzR, 02-Dec-23.</remarks>
    /// <seealso cref="System.Configuration.ConfigurationSection"/>
    ///=================================================================================================
    internal class MLogConfigurationSection : ConfigurationSection
    {
        /// <summary>(Immutable) name of the collection.</summary>
        private const string _collectionName = "RemoteMLogOptions";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>MLog service options.</summary>
        /// <value>Options that control the service.</value>
        ///=================================================================================================
        [ConfigurationProperty(_collectionName)]
        [ConfigurationCollection(typeof(MLogRemoteOptionElementCollection), AddItemName = "option")]
        internal MLogRemoteOptionElementCollection ServiceOptions
            => (MLogRemoteOptionElementCollection)base[_collectionName];
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>MLog remote service configuration element collection.</summary>
    /// <remarks>RzR, 02-Dec-23.</remarks>
    /// <seealso cref="System.Configuration.ConfigurationElementCollection"/>
    ///=================================================================================================
    internal class MLogRemoteOptionElementCollection : ConfigurationElementCollection
    {
        /// <inheritdoc/>
        protected override ConfigurationElement CreateNewElement()
            => new MLogOptionConfigurationElement();

        /// <inheritdoc/>
        protected override object GetElementKey(ConfigurationElement element)
            => ((MLogOptionConfigurationElement)element).Key;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>MLog option configuration element.</summary>
    /// <remarks>RzR, 02-Dec-23.</remarks>
    /// <seealso cref="System.Configuration.ConfigurationElement"/>
    ///=================================================================================================
    internal class MLogOptionConfigurationElement : ConfigurationElement
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Option key.</summary>
        /// <value>The key.</value>
        ///=================================================================================================
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        internal string Key
        {
            get => (string)this["key"];
            set => this["key"] = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Option value.</summary>
        /// <value>The value.</value>
        ///=================================================================================================
        [ConfigurationProperty("value", IsRequired = true)]
        internal string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }
    }
}

#endif