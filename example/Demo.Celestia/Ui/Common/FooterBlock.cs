using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace Demo.Celestia.Ui.Common
{
    /// <summary>
    /// Common complex UI controls block which could be reused across different places.<br/>
    /// Any block could be a PageObject for other child controls.<br/>
    /// Any block could have default locator and name which are used if no direct locator/name is specified in PageObject.
    /// </summary>
    [Find(Using.Id, "footer"), Name("Page footer")]
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
