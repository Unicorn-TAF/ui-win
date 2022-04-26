using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;

namespace Demo.Celestia.Ui.Pages
{
    public class HomePage : BasePage
    {
        public HomePage() : base(string.Empty, "Celestia: Home")
        {
        }

        [Find(Using.WebCss, "h1 > a")]
        public WebControl HomeLink { get; set; }

        [ByClass("fa-map")]
        public WebControl VirtualTextures { get; set; }

        [ByClass("fa-headphones")]
        public WebControl AudioPlaying { get; set; }

        [ByClass("fa-bullseye")]
        public WebControl Trajectories { get; set; }
    }
}
