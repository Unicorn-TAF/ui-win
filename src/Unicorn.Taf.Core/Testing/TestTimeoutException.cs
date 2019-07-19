using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.Taf.Core.Testing
{
    [Serializable]
    public class TestTimeoutException : Exception
    {
        public TestTimeoutException()
            : base()
        {
        }

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
