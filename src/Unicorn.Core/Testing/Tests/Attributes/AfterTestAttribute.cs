using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AfterTestAttribute : Attribute
    {
        public AfterTestAttribute(bool runAlways = true, bool skipTestsOnFail = false)
        {
            this.RunAlways = runAlways;
            this.SkipTestsOnFail = skipTestsOnFail;
        }

        public bool RunAlways { get; protected set; }

        public bool SkipTestsOnFail { get; protected set; }
    }
}
