using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FeatureAttribute : Attribute
    {
        private string feature;

        public FeatureAttribute(string feature)
        {
            this.feature = feature;
        }

        public string Feature => this.feature;
    }
}
