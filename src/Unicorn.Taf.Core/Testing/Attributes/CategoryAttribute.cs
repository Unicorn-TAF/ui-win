using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign a test to some category.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class with specified category.
        /// </summary>
        /// <param name="category">category name</param>
        public CategoryAttribute(string category)
        {
            Category = category;
        }

        /// <summary>
        /// Gets test category.
        /// </summary>
        public string Category { get; }
    }
}
