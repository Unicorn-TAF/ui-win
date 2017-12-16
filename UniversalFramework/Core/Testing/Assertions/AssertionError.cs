using System;

namespace Unicorn.Core.Testing.Assertions
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
