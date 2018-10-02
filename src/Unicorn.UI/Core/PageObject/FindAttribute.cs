using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FindAttribute : Attribute
    {
        public FindAttribute(Using how, string locator)
        {
            this.Locator = new ByLocator(how, locator);
        }

        public ByLocator Locator { get; protected set; }
    }
}
