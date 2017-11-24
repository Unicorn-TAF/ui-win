using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UIWeb.PageObject
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FindByAttribute : Attribute
    {
        ByLocator Locator { get; }

        public FindByAttribute(ByLocator locator)
        {
            Locator = locator;
        }

    }
}
