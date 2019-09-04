using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Thrown in case when test reached execution timeout.
    /// </summary>
    [Serializable]
    public class TestTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestTimeoutException"/>.
        /// </summary>
        public TestTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTimeoutException"/> with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public TestTimeoutException(string exception)
            : base(exception)
        {
        }

        protected TestTimeoutException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
