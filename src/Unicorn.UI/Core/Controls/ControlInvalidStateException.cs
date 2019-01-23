using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.UI.Core.Controls
{
    [Serializable]
    public class ControlInvalidStateException : Exception
    {
        public ControlInvalidStateException()
            : base()
        {
        }

        public ControlInvalidStateException(string exception)
            : base(exception)
        {
        }

        protected ControlInvalidStateException(SerializationInfo info, StreamingContext context) 
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
