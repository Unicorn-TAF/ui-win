using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParametersSetAttribute : Attribute
    {
        public TestSuiteParametersSet ParametersSet;

        public ParametersSetAttribute(string setName, params object[] parameters)
        {
            ParametersSet = new TestSuiteParametersSet(setName, parameters);
        }
    }
}
