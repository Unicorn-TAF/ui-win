using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DefineAttribute : Attribute
    {
        public DefineAttribute(int subElement, Using how, string locator)
        {
            this.ElementDef = subElement;
            this.Locator = new ByLocator(how, locator);
        }

        public ByLocator Locator { get; protected set; }

        public int ElementDef { get; protected set; }
    }
}
