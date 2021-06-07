using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// Attribute is used with dynamically defined drids (<see cref="IDynamicGrid"/>) to define their parts. 
    /// Multiple attributes could be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class DefineGridAttribute : DefineAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefineAttribute"/> class with data grid sub-element definition and locator.
        /// </summary>
        /// <param name="subElement">data grid sub-control definition</param>
        /// <param name="how">locator type</param>
        /// <param name="locator">locator value</param>
        public DefineGridAttribute(GridElement subElement, Using how, string locator)
            : base((int)subElement, how, locator)
        {
        }
    }
}
