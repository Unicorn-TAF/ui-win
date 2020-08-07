using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to add tags to test suites.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TagAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagAttribute"/> class with specified tag name.
        /// </summary>
        /// <param name="tag">tag name</param>
        public TagAttribute(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets or sets tag name.
        /// </summary>
        public string Tag { get; protected set; }
    }
}
