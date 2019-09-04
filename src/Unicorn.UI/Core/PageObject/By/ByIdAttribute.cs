using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject.By
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ByIdAttribute : FindAttribute
    {
        public ByIdAttribute(string locator) : base(Using.Id, locator)
        {
        }
    }
}
