using System;

namespace Unicorn.Taf.Core.Steps.Attributes
{
    /// <summary>
    /// Used to mark specific method within class as test step to use framework test step feature and additional capabilities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class StepAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepAttribute"/> class with specific title.
        /// </summary>
        /// <param name="description">test step title</param>
        public StepAttribute(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Gets value of test step title.
        /// </summary>
        public string Description { get; }
    }
}
