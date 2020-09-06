using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to add some metadata to test suite.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class MetadataAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataAttribute"/> class with specified key and value.
        /// </summary>
        /// <param name="key">metadata key</param>
        /// <param name="value">metadata value</param>
        public MetadataAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets or sets metadata entry key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets metadata entry value.
        /// </summary>
        public string Value { get; }
    }
}
