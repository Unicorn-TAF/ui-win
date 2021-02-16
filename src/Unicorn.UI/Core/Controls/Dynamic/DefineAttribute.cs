using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DefineAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefineAttribute"/> class with element definition and locator.
        /// </summary>
        /// <param name="subElement">sub-control definition as integer index</param>
        /// <param name="how">locator type</param>
        /// <param name="locator">locator value</param>
        public DefineAttribute(int subElement, Using how, string locator)
        {
            ElementDefinition = subElement;
            Locator = new ByLocator(how, locator);
        }

        /// <summary>
        /// Gets or sets locator.
        /// </summary>
        public ByLocator Locator { get; protected set; }

        /// <summary>
        /// Gets or sets element definition as integer index.
        /// </summary>
        public int ElementDefinition { get; protected set; }
    }
}
