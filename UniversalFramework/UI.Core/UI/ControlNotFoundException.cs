using System;

namespace Unicorn.UI.Core.UI
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
