﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase.Pool;
using SuperSocket.SocketBase.Command;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// Extensions class for SocketBase project
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the app server instance in the bootstrap by name, ignore case
        /// </summary>
        /// <param name="bootstrap">The bootstrap.</param>
        /// <param name="name">The name of the appserver instance.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IWorkItem GetServerByName(this IBootstrap bootstrap, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return bootstrap.AppServers.FirstOrDefault(s => name.Equals(s.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the status info metadata from the server type.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static StatusInfoAttribute[] GetStatusInfoMetadata(this Type serverType)
        {
            if (serverType == null)
                throw new ArgumentNullException("serverType");

            var attType = typeof(AppServerMetadataTypeAttribute);

            while (true)
            {
                var atts = serverType.GetCustomAttributes(attType, false);

                if (atts != null && atts.Length > 0)
                {
                    var serverMetadataTypeAtt = atts[0] as AppServerMetadataTypeAttribute;
                    return serverMetadataTypeAtt
                            .MetadataType
                            .GetCustomAttributes(typeof(StatusInfoAttribute), true)
                            .OfType<StatusInfoAttribute>().ToArray();
                }

                if (serverType.BaseType == null)
                    return null;

                serverType = serverType.BaseType;
            }
        }

        /// <summary>
        /// Creates the default pool item creator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool">The pool.</param>
        /// <returns></returns>
        public static IPoolItemCreator<T> CreateDefaultPoolItemCreator<T>(this IPool<T> pool)
            where T : new()
        {
            return new DefaultConstructorItemCreator<T>();
        }

        /// <summary>
        /// Gets the command key from the command instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Command key definition was not found.</exception>
        public static object GetCommandKey<TKey>(this ICommand command)
        {
            var cmdAtt = command.GetType().GetCustomAttributes(true).OfType<CommandAttribute>().FirstOrDefault();

            if (cmdAtt != null)
                return cmdAtt.Key;

            if (typeof(TKey) != typeof(string))
                throw new Exception("Command key definition was not found.");

            return command.Name;
        }
    }
}
