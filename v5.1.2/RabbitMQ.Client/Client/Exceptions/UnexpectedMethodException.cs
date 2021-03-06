using System;

namespace RabbitMQ.Client.Exceptions
{
    /// <summary>
    /// Thrown when the model receives an RPC reply that it wasn't expecting.
    /// </summary>
    public class UnexpectedMethodException : ProtocolViolationException
    {
        public UnexpectedMethodException(IMethod method)
        {
            Method = method;
        }

        public UnexpectedMethodException(string message) : base(message)
        {
        }

        public UnexpectedMethodException(string message, Exception inner) : base(message, inner)
        {
        }

        public UnexpectedMethodException()
        {
        }

        ///<summary>The unexpected reply method.</summary>
        public IMethod Method { get; }
    }
}
