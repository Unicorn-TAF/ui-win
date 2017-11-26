using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FindAttribute : Attribute
    {
        public ByLocator Locator;

        public FindAttribute(Using how, string locator)
        {
            Locator = new ByLocator(how, locator);
        }

    }
}
