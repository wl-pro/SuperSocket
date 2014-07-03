﻿using System;
using System.Collections.Generic;
using System.Text;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocket.Facility.PolicyServer
{
    /// <summary>
    /// PolicyReceiveFilter
    /// </summary>
    class PolicyReceiveFilter : FixedSizeReceiveFilter<StringPackageInfo>
    {
        private const string m_DefaultRequestInfoKey = "REQU";

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyReceiveFilter"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public PolicyReceiveFilter(int size)
            : base(size)
        {

        }

        public override StringPackageInfo ResolvePackage(IList<ArraySegment<byte>> packageData)
        {
            return new StringPackageInfo(m_DefaultRequestInfoKey, Encoding.UTF8.GetString(packageData), null);
        }
    }
}
