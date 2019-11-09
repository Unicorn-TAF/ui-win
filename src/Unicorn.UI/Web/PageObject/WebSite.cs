using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Logging;
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
        /// <param name="baseUrl">site base url</param>
        protected WebSite(string baseUrl)
        {
            BaseUrl = baseUrl.TrimEnd('/');
            _pagesCache = new Dictionary<Type, WebPage>();
        }

        /// <summary>
        /// Gets or sets site base url.
        /// </summary>
        public string BaseUrl { get; protected set; }

        /// <summary>
        /// Open web site (navigate to site base url).
        /// </summary>
        public virtual void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {BaseUrl} site");
            WebDriver.Instance.Get(BaseUrl);
        }

        /// <summary>
        /// Get specified site page instance.
        /// </summary>
        /// <typeparam name="T">page type</typeparam>
        /// <returns>page instance</returns>
        public T GetPage<T>() where T : WebPage
        {
            var type = typeof(T);

            if (!_pagesCache.ContainsKey(type))
            {
                var page = Activator.CreateInstance<T>();
                _pagesCache.Add(type, page);
            }

            return _pagesCache[type] as T;
        }

        /// <summary>
        /// Navigate to specified site page.
        /// </summary>
        /// <typeparam name="T">page type</typeparam>
        /// <returns>page instance</returns>
        public virtual T NavigateTo<T>() where T : WebPage
        {
            var page = GetPage<T>();
            WebDriver.Instance.Get($"{BaseUrl}/{page.Url.TrimStart('/')}");
            return page;
        }

        /// <summary>
        /// Reset pages cache (commonly in case when driver context was changed)
        /// </summary>
        public void ResetPagesCache() =>
            _pagesCache.Clear();
    }
}
