using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class DefineGridAttribute : DefineAttribute
    {
        public DefineGridAttribute(GridElement subElement, Using how, string locator)
            : base((int)subElement, how, locator)
        {
        }
    }
}
