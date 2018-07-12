using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebPage : WebContainer
    {
        protected WebPage(string url, string title)
        {
            this.SearchContext = WebDriver.Driver as OpenQA.Selenium.ISearchContext;
            Init();
            this.Url = url;
            this.Title = title;
        }

        public bool Opened
        {
            get
            {
                bool opened = WebDriver.Instance.Url.EndsWith(this.Url);

                if (!string.IsNullOrEmpty(this.Title))
                {
                    opened &= WebDriver.Driver.Title.Equals(this.Title);
                }

                return opened;
            }
        } 

        public string Url { get; protected set; }

        public string Title { get; protected set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Title) ? base.ToString() : this.Title;
        }
    }
}
