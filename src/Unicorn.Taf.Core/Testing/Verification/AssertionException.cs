using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Unicorn.Taf.Core.Testing.Verification
{
    [Serializable]
    public class AssertionException : Exception
    {
        public AssertionException() : base()
        {
        }

        public AssertionException(string message) : base(message)
        {
        }

        protected AssertionException(SerializationInfo info, StreamingContext context) 
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
