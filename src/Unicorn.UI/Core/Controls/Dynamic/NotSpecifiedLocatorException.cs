using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// Thrown in case when UI control was not found by search.
    /// </summary>
    [Serializable]
    public class NotSpecifiedLocatorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecifiedLocatorException"/> class.
        /// </summary>
        public NotSpecifiedLocatorException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecifiedLocatorException"/> class with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public NotSpecifiedLocatorException(string exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecifiedLocatorException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected NotSpecifiedLocatorException(SerializationInfo info, StreamingContext context) 
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
