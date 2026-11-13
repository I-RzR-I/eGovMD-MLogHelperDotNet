// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MLogHelperDotNet
//  Author           : RzR
//  Created On       : 2026-11-11 22:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-11-11 22:59
// ***********************************************************************
//  <copyright file="CappedStream.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using MLogHelperDotNet.Helpers;
using System.IO;

#endregion

namespace MLogHelperDotNet.Models
{
    /// <summary>
    ///     A memory stream wrapper that counts every byte written against the per-property output
    ///     cap.
    /// </summary>
    /// <seealso cref="T:System.IO.Stream"/>
    internal sealed class CappedStream : Stream
    {
        /// <summary>(Immutable) the inner stream.</summary>
        private readonly MemoryStream inner = new MemoryStream();

        /// <summary>(Immutable) the per-property serialization context.</summary>
        private readonly SerializationContext context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CappedStream" /> class.
        /// </summary>
        /// <param name="context">The per-property serialization context.</param>
        public CappedStream(SerializationContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public override bool CanRead => inner.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => inner.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => inner.CanWrite;

        /// <inheritdoc />
        public override long Length => inner.Length;

        /// <inheritdoc />
        public override long Position
        {
            get => inner.Position;
            set => inner.Position = value;
        }

        /// <inheritdoc />
        public override void Flush() => inner.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);

        /// <inheritdoc />
        public override void SetLength(long value) => inner.SetLength(value);

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            PropertyValueSerializerHelper.CountChars(context, count);
            inner.Write(buffer, offset, count);
        }

        /// <summary>
        ///     Gets the written bytes.
        /// </summary>
        /// <returns>
        ///     The written bytes.
        /// </returns>
        public byte[] ToArray() => inner.ToArray();
    }
}