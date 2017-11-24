using System;

namespace Unicorn.UI.Core.UI
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
