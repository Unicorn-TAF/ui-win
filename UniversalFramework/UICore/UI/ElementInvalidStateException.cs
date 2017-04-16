using System;

namespace UICore.UI
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
