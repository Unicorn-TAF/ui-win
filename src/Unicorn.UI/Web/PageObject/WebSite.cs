using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    /// <summary>
    /// Provides base functionality of web site.
    /// </summary>
    public abstract class WebSite
    {
        private readonly Dictionary<Type, WebPage> _pagesCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSite"/> class with specified base url.
        /// </summary>
        /// <param name="driver">instance of web driver</param>
        /// <param name="baseUrl">site base url</param>
        protected WebSite(WebDriver driver, string baseUrl)
        {
            BaseUrl = new Uri(baseUrl);
            _pagesCache = new Dictionary<Type, WebPage>();
            Driver = driver;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSite"/> class with specified base url and browser type.
        /// </summary>
        /// <param name="browserType">browser type to start site in</param>
        /// <param name="baseUrl">site base url</param>
        protected WebSite(BrowserType browserType, string baseUrl)
        {
            BaseUrl = new Uri(baseUrl);
            _pagesCache = new Dictionary<Type, WebPage>();
            Driver = new DesktopWebDriver(browserType, true);
        }

        /// <summary>
        /// Gets WebDriver instance used by the website.
        /// </summary>
        public WebDriver Driver { get; }

        /// <summary>
        /// Gets or sets site base url.
        /// </summary>
        public Uri BaseUrl { get; protected set; }

        /// <summary>
        /// Open web site (navigate to site base url).
        /// </summary>
        public virtual void Open() =>
            Driver.Get(BaseUrl.ToString());

        /// <summary>
        /// Gets specified site page instance from pages cache 
        /// (page should have constructor with <see cref="IWebDriver"/> argument).
        /// </summary>
        /// <typeparam name="T">page type</typeparam>
        /// <returns>page instance</returns>
        public T GetPage<T>() where T : WebPage
        {
            var type = typeof(T);

            if (!_pagesCache.ContainsKey(type))
            {
                T page = (T)Activator.CreateInstance(type, Driver.SeleniumDriver);
                _pagesCache.Add(type, page);
            }

            return _pagesCache[type] as T;
        }

        /// <summary>
        /// Navigates to specified site page.
        /// </summary>
        /// <typeparam name="T">page type</typeparam>
        /// <returns>page instance</returns>
        public virtual T NavigateTo<T>() where T : WebPage
        {
            var page = GetPage<T>();
            Driver.Get(new Uri(BaseUrl, page.Url).ToString());
            return page;
        }
    }
}
