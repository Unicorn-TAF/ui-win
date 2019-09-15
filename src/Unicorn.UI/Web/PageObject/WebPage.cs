using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    /// <summary>
    /// Provides base functionality of web page which is also as <see cref="WebContainer"/> of all child controls.
    /// </summary>
    public abstract class WebPage : WebContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebPage"/> class with specified root search context, page sub-url and title.
        /// </summary>
        /// <param name="searchContext">root search context (usually <see cref="OpenQA.Selenium.IWebDriver"/> instance)</param>
        /// <param name="url">page sub-url</param>
        /// <param name="title">page title</param>
        protected WebPage(OpenQA.Selenium.ISearchContext searchContext, string url, string title)
        {
            this.SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            this.Url = url;
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPage"/> class with specified root search context, empty sub-url and empty title.
        /// </summary>
        /// <param name="searchContext">root search context (usually <see cref="OpenQA.Selenium.IWebDriver"/> instance)</param>
        protected WebPage(OpenQA.Selenium.ISearchContext searchContext) : this(searchContext, string.Empty, string.Empty)
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

        /// <summary>
        /// Gets or sets page sub-url.
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Gets or sets page title.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets or sets page body control.
        /// </summary>
        [Name("Page body")]
        [Find(Using.WebTag, "body")]
        public WebControl Body { get; set; }

        /// <summary>
        /// Gets string description of the web page.
        /// </summary>
        /// <returns>page description as string</returns>
        public override string ToString() =>
            $"page '{(string.IsNullOrEmpty(this.Title) ? GetType().ToString() : this.Title)}'";
    }
}
