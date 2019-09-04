using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject.By
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ByClassAttribute : FindAttribute
    {
        public ByClassAttribute(string locator) : base(Using.Class, locator)
        {
        }
    }
}
