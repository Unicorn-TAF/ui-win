using System.Linq;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject.Attributes;

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
            SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            Url = url;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPage"/> class with specified root search context.
        /// </summary>
        /// <param name="searchContext">root search context (usually <see cref="OpenQA.Selenium.IWebDriver"/> instance)</param>
        protected WebPage(OpenQA.Selenium.ISearchContext searchContext)
        {
            SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            var relativeUrlAttributes = GetType().GetCustomAttributes(typeof(PageInfoAttribute), true) as PageInfoAttribute[];
            
            Url = relativeUrlAttributes.FirstOrDefault()?.RelativeUrl;
            Title = relativeUrlAttributes.FirstOrDefault()?.Title;

        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is opened based on:<para/> 
        ///  - current opened Url (should end with page url if any specified for the page)<para/> 
        ///  - page title (if any specified for the page)<para/> 
        ///  If url and title were not set, page is considered to be opened.
        /// </summary>
        public bool Opened
        {
            get
            {
                bool opened = true;

                if (!string.IsNullOrEmpty(Title))
                {
                    opened &= WebDriver.Instance.Url.EndsWith(Url);
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    opened &= WebDriver.Instance.SeleniumDriver.Title.Equals(Title);
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
            $"page '{(string.IsNullOrEmpty(Title) ? GetType().ToString() : Title)}'";
    }
}
