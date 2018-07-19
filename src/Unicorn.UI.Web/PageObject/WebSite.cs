using System;
using Unicorn.Core.Logging;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebSite
    {
        protected WebSite(BrowserType type, string baseUrl)
        {
            WebDriver.Init(type);
            this.BaseUrl = baseUrl.TrimEnd('/');
        }

        protected WebSite(string baseUrl)
        {
            this.BaseUrl = baseUrl.TrimEnd('/');
        }

        public string BaseUrl { get; protected set; }

        public T GetPage<T>() where T : WebPage
        {
            return Activator.CreateInstance<T>();
        }

        public T NavigateTo<T>() where T : WebPage
        {
            var page = GetPage<T>();
            Logger.Instance.Log(LogLevel.Debug, $"Navigate to {page.Url} page");
            WebDriver.Instance.Get(this.BaseUrl + page.Url);
            return page;
        }

        public void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {this.BaseUrl} site");
            WebDriver.Instance.Get(this.BaseUrl);
        }
    }
}
