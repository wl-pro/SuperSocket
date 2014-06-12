﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// Extentions class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets string from the binary segments data.
        /// </summary>
        /// <param name="encoding">The text encoding to decode the binary data.</param>
        /// <param name="data">The binary segments data.</param>
        /// <returns>the decoded string</returns>
        public static string GetString(this Encoding encoding, IList<ArraySegment<byte>> data)
        {
            var total = data.Sum(x => x.Count);

            var output = new char[encoding.GetMaxCharCount(total)];

            var decoder = encoding.GetDecoder();

            var totalCharsLen = 0;
            var lastIndex = data.Count - 1;
            var bytesUsed = 0;
            var charsUsed = 0;
            var completed = false;

            for (var i = 0; i < data.Count; i++)
            {
                var segment = data[i];

                decoder.Convert(segment.Array, segment.Offset, segment.Count, output, totalCharsLen, output.Length - totalCharsLen, i == lastIndex, out bytesUsed, out charsUsed, out completed);
                totalCharsLen += charsUsed;
            }

            return new string(output, 0, totalCharsLen);
        }

        /// <summary>
        /// Gets string from the binary segments data.
        /// </summary>
        /// <param name="encoding">The text encoding to decode the binary data.</param>
        /// <param name="data">The binary segments data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>
        /// the decoded string
        /// </returns>
        public static string GetString(this Encoding encoding, IList<ArraySegment<byte>> data, int offset, int length)
        {
            var output = new char[encoding.GetMaxCharCount(length)];

            var decoder = encoding.GetDecoder();

            var totalCharsLen = 0;
            var totalBytesLen = 0;
            var lastIndex = data.Count - 1;
            var bytesUsed = 0;
            var charsUsed = 0;
            var completed = false;

            var targetOffset = 0;

            for (var i = 0; i < data.Count; i++)
            {
                var segment = data[i];
                var srcOffset = segment.Offset;
                var srcLength = segment.Count;
                var lastSegment = false;

                //Haven't found the offset position
                if (totalBytesLen == 0)
                {
                    var targetEndOffset = targetOffset + segment.Count - 1;

                    if (offset > targetEndOffset)
                    {
                        targetOffset = targetEndOffset + 1;
                        continue;
                    }

                    //the offset locates in this segment
                    var margin = offset - targetOffset;
                    srcOffset = srcOffset + margin;
                    srcLength = srcLength - margin;

                    if (srcLength >= length)
                    {
                        srcLength = length;
                        lastSegment = true;
                    }
                }
                else
                {
                    var restLength = length - totalBytesLen;

                    if (restLength <= srcLength)
                    {
                        srcLength = restLength;
                        lastSegment = true;
                    }
                }

                decoder.Convert(segment.Array, srcOffset, srcLength, output, totalCharsLen, output.Length - totalCharsLen, lastSegment, out bytesUsed, out charsUsed, out completed);
                totalCharsLen += charsUsed;
                totalBytesLen += bytesUsed;
            }

            return new string(output, 0, totalCharsLen);
        }

        /// <summary>
        /// Gets a buffer reader instance which can be reused.
        /// </summary>
        /// <typeparam name="TPackageInfo">The type of the package info.</typeparam>
        /// <param name="receiveFilter">The receive filter.</param>
        /// <param name="data">The buffer data source.</param>
        /// <returns></returns>
        public static IBufferReader GetBufferReader<TPackageInfo>(this IReceiveFilter<TPackageInfo> receiveFilter, IList<ArraySegment<byte>> data)
            where TPackageInfo : IPackageInfo
        {
            return GetBufferReader<BufferListReader, TPackageInfo>(receiveFilter, data);
        }

        /// <summary>
        /// Gets a buffer reader instance which can be reused.
        /// </summary>
        /// <typeparam name="TReader">The type of the reader.</typeparam>
        /// <typeparam name="TPackageInfo">The type of the package info.</typeparam>
        /// <param name="receiveFilter">The receive filter.</param>
        /// <param name="data">The buffer data source.</param>
        /// <returns></returns>
        public static IBufferReader GetBufferReader<TReader, TPackageInfo>(this IReceiveFilter<TPackageInfo> receiveFilter, IList<ArraySegment<byte>> data)
            where TReader : BufferListReader, new()
            where TPackageInfo : IPackageInfo
        {
            var reader = BufferListReader.GetCurrent<TReader>();
            reader.Initialize(data);
            return reader;
        }
    }
}
