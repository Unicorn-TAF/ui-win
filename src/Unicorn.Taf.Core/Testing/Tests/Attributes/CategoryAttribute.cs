using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

        public string Category { get; protected set; }
    }
}
