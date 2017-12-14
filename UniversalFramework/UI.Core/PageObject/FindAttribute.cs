using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FindAttribute : Attribute
    {
        private ByLocator locator;

        public FindAttribute(Using how, string locator)
        {
            this.locator = new ByLocator(how, locator);
        }

        public ByLocator Locator => this.locator;
    }
}
