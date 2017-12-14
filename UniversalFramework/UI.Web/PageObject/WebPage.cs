using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebPage : WebContainer
    {
        public WebPage()
        {
            this.SearchContext = WebDriver.Driver as OpenQA.Selenium.ISearchContext;
            Init();
        }

        public abstract void WaitForPageLoading();
    }
}
