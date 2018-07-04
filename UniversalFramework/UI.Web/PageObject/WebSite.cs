using System;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    public abstract class WebSite
    {
        protected WebSite(string baseUrl)
        {
            this.BaseUrl = baseUrl.TrimEnd('/');
        }

        public string BaseUrl { get; protected set; }

        public T GetPage<T>() where T : WebPage
        {
            return Activator.CreateInstance<T>();
        }

        public T NavigateTo<T>(T pageInstance) where T : WebPage
        {
            WebDriver.Instance.Get(this.BaseUrl + pageInstance.Url);
            return pageInstance;
        }

        public T NavigateTo<T>() where T : WebPage
        {
            var page = GetPage<T>();
            WebDriver.Instance.Get(this.BaseUrl + page.Url);
            return page;
        }

        public void Open()
        {
            WebDriver.Instance.Get(this.BaseUrl);
        }
    }
}
