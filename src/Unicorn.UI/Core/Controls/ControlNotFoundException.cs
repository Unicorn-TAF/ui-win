using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.UI.Core.Controls
{
    [Serializable]
    public class ControlNotFoundException : Exception
    {
        public ControlNotFoundException()
            : base()
        {
        }

        public ControlNotFoundException(string exception)
            : base(exception)
        {
        }

        protected ControlNotFoundException(SerializationInfo info, StreamingContext context) 
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
