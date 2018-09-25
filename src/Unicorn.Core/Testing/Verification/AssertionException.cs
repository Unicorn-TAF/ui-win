using System;

namespace Unicorn.Core.Testing.Verification
{
    public class AssertionException : Exception
    {
        public AssertionException() : base()
        {
        }

        public AssertionException(string message) : base(message)
        {
        }
    }
}
