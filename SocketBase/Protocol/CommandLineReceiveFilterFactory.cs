﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// CommandLine RequestFilter Factory
    /// </summary>
    public class CommandLineReceiveFilterFactory : TerminatorReceiveFilterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineReceiveFilterFactory"/> class.
        /// </summary>
        public CommandLineReceiveFilterFactory()
            : this(Encoding.ASCII)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineReceiveFilterFactory"/> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        public CommandLineReceiveFilterFactory(Encoding encoding)
            : this(encoding, new BasicPackageInfoParser())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineReceiveFilterFactory"/> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="requestInfoParser">The request info parser.</param>
        public CommandLineReceiveFilterFactory(Encoding encoding, IStringPackageParser<StringPackageInfo> requestInfoParser)
            : base("\r\n", encoding, requestInfoParser)
        {

        }
    }
}
