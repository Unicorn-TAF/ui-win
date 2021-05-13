using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Thrown in case when test suite reached execution timeout.
    /// </summary>
    [Serializable]
    public class SuiteTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteTimeoutException"/> class.
        /// </summary>
        public SuiteTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteTimeoutException"/> class with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public SuiteTimeoutException(string exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteTimeoutException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected SuiteTimeoutException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        /// <summary>
        /// Set serialization info
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
