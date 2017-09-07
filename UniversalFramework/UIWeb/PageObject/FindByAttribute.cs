using System;
using Unicorn.UICore.Driver;

namespace Unicorn.UIWeb.PageObject
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FindByAttribute : Attribute
    {
        public FindByAttribute(By by, string locator)
        {
            By = by;
            Locator = locator;
        }

        public By By { get; set; }

        public string Locator { get; set; }
    }
}
