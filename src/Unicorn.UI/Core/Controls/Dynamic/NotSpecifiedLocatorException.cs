using System;
using System.Runtime.Serialization;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// Thrown in case when dynamic UI control definition has some missing element required by specific method or call.
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
    }
}
