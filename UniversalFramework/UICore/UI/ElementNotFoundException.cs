using System;

namespace Unicorn.UICore.UI
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException()
            : base()
        { }

        public ElementNotFoundException(string exception)
            : base(exception)
        { }
    }
}
