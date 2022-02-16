using System;
using System.Runtime.Serialization;

namespace Unicorn.Taf.Core.Verification
{
    /// <summary>
    /// Thrown in case when assert is failed.
    /// </summary>
    [Serializable]
    public class AssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class.
        /// </summary>
        public AssertionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class with specified message.
        /// </summary>
        /// <param name="message">exception message</param>
        public AssertionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected AssertionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
