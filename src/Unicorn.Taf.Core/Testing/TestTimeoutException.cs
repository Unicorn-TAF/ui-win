using System;
using System.Runtime.Serialization;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Thrown in case when test reached execution timeout.
    /// </summary>
    [Serializable]
    public class TestTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestTimeoutException"/> class.
        /// </summary>
        public TestTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTimeoutException"/> class with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public TestTimeoutException(string exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTimeoutException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected TestTimeoutException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
