using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject.By
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ByNameAttribute : FindAttribute
    {
        public ByNameAttribute(string locator) : base(Using.Name, locator)
        {
        }
    }
}
