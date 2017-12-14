using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CategoryAttribute : Attribute
    {
        private string category;

        public CategoryAttribute(string category)
        {
            this.category = category;
        }

        public string Category => this.category;
    }
}
