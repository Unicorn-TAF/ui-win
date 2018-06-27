using System;

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
    }
}
