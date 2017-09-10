using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FeatureAttribute : Attribute
    {
        private string featureName;

        public FeatureAttribute(string featureName)
        {
            this.featureName = featureName;
        }

        public string FeatureName
        {
            get
            {
                return featureName;
            }
        }
    }
}
