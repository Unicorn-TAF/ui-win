using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebPage : WebContainer
    {
        protected WebPage()
        {
            this.SearchContext = WebDriver.Driver as OpenQA.Selenium.ISearchContext;
            Init();
        }

        public bool IsOpened
        {
            get
            {
                bool isOpened = WebDriver.Instance.Url.EndsWith(this.Url);

                if (!string.IsNullOrEmpty(this.Title))
                {
                    isOpened &= WebDriver.Driver.Title.Equals(this.Title);
                }

                return isOpened;
            }
        } 

        public abstract string Url { get; }

        public abstract string Title { get; }
    }
}
