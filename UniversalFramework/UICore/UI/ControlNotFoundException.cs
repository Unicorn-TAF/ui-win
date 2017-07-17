using System;

namespace Unicorn.UICore.UI
{
    public class ControlNotFoundException : Exception
    {
        public ControlNotFoundException()
            : base()
        { }

        public ControlNotFoundException(string exception)
            : base(exception)
        { }
    }
}
