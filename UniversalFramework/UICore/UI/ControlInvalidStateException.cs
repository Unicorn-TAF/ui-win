using System;

namespace Unicorn.UICore.UI
{
    public class ControlInvalidStateException : Exception
    {
            public ControlInvalidStateException()
                : base()
            { }

            public ControlInvalidStateException(string exception)
                : base(exception)
            { }
    }
}
