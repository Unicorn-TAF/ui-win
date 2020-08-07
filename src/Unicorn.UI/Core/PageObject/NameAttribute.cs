using System;

namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Provides with ability to assign readable name to controls
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameAttribute"/> class with specified name.
        /// </summary>
        /// <param name="name">control name</param>
        public NameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets control name.
        /// </summary>
        public string Name
        {
            get;
            
            protected set;
        }
    }
}
