using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FeatureAttribute : Attribute
    {
        public FeatureAttribute(string feature)
        {
            this.Feature = feature;
        }

        public string Feature { get; protected set; }
    }
}
