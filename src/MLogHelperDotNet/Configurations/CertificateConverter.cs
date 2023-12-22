// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:17
// ***********************************************************************
//  <copyright file="CertificateConverter.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

#endregion

namespace MLogHelperDotNet.Configurations
{
    /// <summary>
    ///     Certificate converter
    /// </summary>
    public class CertificateConverter : TypeConverter
    {
        /// <summary>
        ///     Register
        /// </summary>
        public static void Register() => TypeDescriptor.AddAttributes(typeof(X509Certificate2),
            new TypeConverterAttribute(typeof(CertificateConverter)));

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value as string;
            if (text.IsNull() || string.IsNullOrWhiteSpace(text))
            {
                return base.ConvertFrom(context, culture, value);
            }

            var num = text.IndexOf('|');
            if (num.IsLessZero())
            {
                return CertificateLoader.Public(text);
            }

            var text2 = text;
            var num2 = num - 0;
            var certificatePath = text2.Substring(0, num2);
            var text3 = text;
            var length = text3.Length;
            num2 = num + 1;
            var length2 = length - num2;

            return CertificateLoader.Private(certificatePath, text3.Substring(num2, length2));
        }
    }
}