using System;

namespace Unicorn.UI.Core.Controls
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
