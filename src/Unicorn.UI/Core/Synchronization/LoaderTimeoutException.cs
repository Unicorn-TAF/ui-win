using System;
using System.Runtime.Serialization;

namespace Unicorn.UI.Core.Synchronization
{
    /// <summary>
    /// Thrown in case when UI loader has not disappeared.
    /// </summary>
    [Serializable]
    public class LoaderTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderTimeoutException"/> class.
        /// </summary>
        public LoaderTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderTimeoutException"/> class with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public LoaderTimeoutException(string exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderTimeoutException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected LoaderTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}