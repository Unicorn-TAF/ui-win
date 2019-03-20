using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AfterTestAttribute : Attribute
    {
        public AfterTestAttribute() : this(true, false)
        {
        }

        public AfterTestAttribute(bool runAlway) : this(runAlway, false)
        {
        }

        public AfterTestAttribute(bool runAlways, bool skipTestsOnFail)
        {
            this.RunAlways = runAlways;
            this.SkipTestsOnFail = skipTestsOnFail;
        }

        public bool RunAlways { get; protected set; }

        public bool SkipTestsOnFail { get; protected set; }
    }
}
