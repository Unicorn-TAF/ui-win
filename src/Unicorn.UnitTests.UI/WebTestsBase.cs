using OpenQA.Selenium;
using System;
using Unicorn.UI.Web.PageObject;

namespace Unicorn.UnitTests.UI
{
    public class WebTestsBase
    {
        protected static T NavigateToPage<T>(IWebDriver driver) where T : WebPage
        {
            T page = (T)Activator.CreateInstance(typeof(T), new object[] { driver });
            driver.Url = page.Url;
            return page;
        }
    }
}
