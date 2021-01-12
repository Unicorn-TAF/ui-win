using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Provides with ability to specify search condition for UI control PageObject
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class FindAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAttribute"/> class with specified search method and locator
        /// </summary>
        /// <param name="how">search method</param>
        /// <param name="locator">locator to search by</param>
        public FindAttribute(Using how, string locator)
        {
            Locator = new ByLocator(how, locator);
        }

        /// <summary>
        /// Gets or sets control locator.
        /// </summary>
        public ByLocator Locator 
        { 
            get; 

            protected set; 
        }
    }
}
