using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AfterTestAttribute : Attribute
    {
        private bool runAlways, skipTestsOnFail;

        public AfterTestAttribute(bool runAlways = true, bool skipTestsOnFail = false)
        {
            this.runAlways = runAlways;
            this.skipTestsOnFail = skipTestsOnFail;
        }

        public bool RunAlways => this.runAlways;

        public bool SkipTestsOnFail => this.skipTestsOnFail;
    }
}
