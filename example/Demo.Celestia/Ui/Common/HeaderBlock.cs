using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;

namespace Demo.Celestia.Ui.Common
{
    /// <summary>
    /// Generic complex UI controls block which could be reused across different places.
    /// </summary>
    public class HeaderBlock : WebContainer
    {
        [ById("logo"), Name("Logo")]
        public WebControl Logo { get; set; }

        public WebControl GetNavItem(string itemName) =>
            Find<WebControl>(ByLocator.Xpath($".//nav/ul/li/a[text() = '{itemName}']"));
    }
}
