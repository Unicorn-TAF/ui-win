using System;
using System.Runtime.Serialization;

namespace Unicorn.UI.Core.Controls
{
    /// <summary>
    /// Thrown in case when UI control was not found by search.
    /// </summary>
    [Serializable]
    public class ControlNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class.
        /// </summary>
        public ControlNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class with specified message.
        /// </summary>
        /// <param name="exception">exception message</param>
        public ControlNotFoundException(string exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlNotFoundException"/> class with specified serialization info and context.
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        protected ControlNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
