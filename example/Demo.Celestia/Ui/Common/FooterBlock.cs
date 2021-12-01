using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace Demo.Celestia.Ui.Common
{
    /// <summary>
    /// Generic complex UI controls block which could be reused across different places.
    /// Any block could be a PageObject for other child controls.
    /// Any block could have default locator with will be used if no direct locator is specified in PageObject.
    /// </summary>
    [Find(Using.Id, "footer")]
    public class FooterBlock : WebContainer
    {
        [Name("Twitter link")]
        [Find(Using.WebCss, "a.fa-twitter")]
        public WebControl LinkTwitter { get; set; }

        [Name("GitHub link")]
        [Find(Using.WebCss, "a.fa-github")]
        public WebControl LinkGithub { get; set; }

        [Name("Email link")]
        [Find(Using.WebCss, "a.fa-envelope")]
        public WebControl LinkEmail { get; set; }

        [Name("Copyright")]
        [Find(Using.WebCss, "ul.copyright>li")]
        public WebControl Copyright { get; set; }
    }
}
