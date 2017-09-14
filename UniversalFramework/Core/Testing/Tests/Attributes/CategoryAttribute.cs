using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CategoryAttribute : Attribute
    {
        private string _category;

        public CategoryAttribute(string category)
        {
            _category = category;
        }

        public string Category
        {
            get
            {
                return _category;
            }
        }
    }
}
