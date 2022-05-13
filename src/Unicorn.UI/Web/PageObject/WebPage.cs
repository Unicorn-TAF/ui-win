using System.Reflection;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.PageObject.Attributes;
using Selenium = OpenQA.Selenium;

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
        /// <param name="searchContext">root search context (usually <see cref="Selenium.IWebDriver"/> instance)</param>
        /// <param name="url">page sub-url</param>
        /// <param name="title">page title</param>
        protected WebPage(Selenium.IWebDriver searchContext, string url, string title)
        {
            SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            Url = url;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPage"/> class with specified root search context.
        /// </summary>
        /// <param name="searchContext">root search context (usually <see cref="Selenium.IWebDriver"/> instance)</param>
        protected WebPage(Selenium.IWebDriver searchContext)
        {
            SearchContext = searchContext;
            ContainerFactory.InitContainer(this);
            PageInfoAttribute relativeUrlAttribute = GetType().GetCustomAttribute<PageInfoAttribute>(true);
            Url = relativeUrlAttribute?.RelativeUrl;
            Title = relativeUrlAttribute?.Title;
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

                Selenium.IWebDriver driver = (Selenium.IWebDriver)SearchContext;

                if (!string.IsNullOrEmpty(Url))
                {
                    opened &= driver.Url.EndsWith(Url);
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    opened &= driver.Title.Equals(Title);
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
