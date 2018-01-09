using System;

namespace Unicorn.Core.Testing.Verification
{
    [Serializable]
    public class AssertionError : Exception
    {
        public AssertionError() : base()
        {
        }

        public AssertionError(string message) : base(message)
        {
        }
    }
}
