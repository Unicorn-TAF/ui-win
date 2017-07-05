using System;

namespace Unicorn.UICore.UI
{
    public class ElementInvalidStateException : Exception
    {
            public ElementInvalidStateException()
                : base()
            { }

            public ElementInvalidStateException(string exception)
                : base(exception)
            { }
    }
}
