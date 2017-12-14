using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParametersSetAttribute : Attribute
    {
        public ParametersSetAttribute(string setName, params object[] parameters)
        {
            this.ParametersSet = new TestSuiteParametersSet(setName, parameters);
        }

        public TestSuiteParametersSet ParametersSet
        {
            get;

            set;
        }
    }
}
