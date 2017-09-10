using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        private string suiteName;

        public TestSuiteAttribute(string suiteName)
        {
            this.suiteName = suiteName;
        }

        public string SuiteName
        {
            get
            {
                return suiteName;
            }
        }
    }
}
