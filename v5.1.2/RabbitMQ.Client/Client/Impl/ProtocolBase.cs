using RabbitMQ.Client.Impl;
using RabbitMQ.Util;
using System.Collections.Generic;

namespace RabbitMQ.Client.Framing.Impl
{
    public abstract class ProtocolBase : IProtocol
    {
        public IDictionary<string, bool> Capabilities = new Dictionary<string, bool>();

        protected ProtocolBase()
        {
            Capabilities["publisher_confirms"] = true;
            Capabilities["exchange_exchange_bindings"] = true;
            Capabilities["basic.nack"] = true;
            Capabilities["consumer_cancel_notify"] = true;
            Capabilities["connection.blocked"] = true;
            Capabilities["authentication_failure_close"] = true;
        }

        public abstract string ApiName { get; }
        public abstract int DefaultPort { get; }

        public abstract int MajorVersion { get; }
        public abstract int MinorVersion { get; }
        public abstract int Revision { get; }

        public AmqpVersion Version
        {
            get { return new AmqpVersion(MajorVersion, MinorVersion); }
        }

        public bool CanSendWhileClosed(Command cmd)
        {
            return cmd.Method is ChannelCloseOk;
        }

        public void CreateChannelClose(ushort reasonCode,
            string reasonText,
            out Command request,
            out int replyClassId,
            out int replyMethodId)
        {
            request = new Command(new ChannelClose(reasonCode,
                reasonText,
                0, 0));
            replyClassId = ChannelCloseOk.ClassId;
            replyMethodId = ChannelCloseOk.MethodId;
        }

        public void CreateConnectionClose(ushort reasonCode,
            string reasonText,
            out Command request,
            out int replyClassId,
            out int replyMethodId)
        {
            request = new Command(new ConnectionClose(reasonCode,
                reasonText,
                0, 0));
            replyClassId = ConnectionCloseOk.ClassId;
            replyMethodId = ConnectionCloseOk.MethodId;
        }

        public abstract ContentHeaderBase DecodeContentHeaderFrom(NetworkBinaryReader reader);
        public abstract MethodBase DecodeMethodFrom(NetworkBinaryReader reader);

        public override bool Equals(object obj)
        {
            return GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public override string ToString()
        {
            return Version.ToString();
        }

        public IConnection CreateConnection(IConnectionFactory factory,
            bool insist,
            IFrameHandler frameHandler)
        {
            return new Connection(factory, insist, frameHandler, null);
        }

        public IConnection CreateConnection(IConnectionFactory factory,
            bool insist,
            IFrameHandler frameHandler,
            string clientProvidedName)
        {
            return new Connection(factory, insist, frameHandler, clientProvidedName);
        }

        public IConnection CreateConnection(ConnectionFactory factory,
            IFrameHandler frameHandler,
            bool automaticRecoveryEnabled)
        {
            var ac = new AutorecoveringConnection(factory, null);
            ac.Init();
            return ac;
        }

        public IConnection CreateConnection(ConnectionFactory factory,
            IFrameHandler frameHandler,
            bool automaticRecoveryEnabled,
            string clientProvidedName)
        {
            var ac = new AutorecoveringConnection(factory, clientProvidedName);
            ac.Init();
            return ac;
        }

        public IModel CreateModel(ISession session)
        {
            return new Model(session);
        }

        public IModel CreateModel(ISession session, ConsumerWorkService workService)
        {
            return new Model(session, workService);
        }
    }
}