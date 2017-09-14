using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FeatureAttribute : Attribute
    {
        private string _feature;

        public FeatureAttribute(string feature)
        {
            _feature = feature;
        }

        public string Feature
        {
            get
            {
                return _feature;
            }
        }
    }
}
