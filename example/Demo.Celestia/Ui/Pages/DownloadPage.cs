using System.Collections.Generic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Demo.Celestia.Ui.Pages
{
    public class DownloadPage : BasePage
    {
        public DownloadPage() : base("/download.html", "Celestia: Download")
        {
        }

        public IList<WebControl> DownloadsList => FindList<WebControl>(ByLocator.Css("section#content i"));
    }
}
