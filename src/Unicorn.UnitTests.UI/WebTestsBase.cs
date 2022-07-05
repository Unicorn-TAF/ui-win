using OpenQA.Selenium;
using System;
using Unicorn.UI.Web.PageObject;

namespace Unicorn.UnitTests.UI
{
    public class WebTestsBase
    {
        protected static T NavigateToPage<T>(IWebDriver driver, bool forceNavigation) where T : WebPage
        {
            T page = (T)Activator.CreateInstance(typeof(T), new object[] { driver });

            if (forceNavigation || driver.Url != page.Url)
            {
                driver.Url = page.Url;
            }

            return page;
        }

        protected static T NavigateToPage<T>(IWebDriver driver) where T : WebPage =>
            NavigateToPage<T>(driver, false);
    }
}
