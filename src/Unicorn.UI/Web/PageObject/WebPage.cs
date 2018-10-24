using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebPage : WebContainer
    {
        protected WebPage(OpenQA.Selenium.ISearchContext searchContext, string url, string title)
        {
            this.SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            this.Url = url;
            this.Title = title;
        }

        protected WebPage(OpenQA.Selenium.ISearchContext searchContext) : this (searchContext, string.Empty, string.Empty)
        {
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

        [Find(Using.Web_Tag, "body")]
        public WebControl Body { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Title) ? base.ToString() : this.Title;
        }
    }
}
