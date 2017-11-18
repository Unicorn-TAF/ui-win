using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UICore.Driver;
using Unicorn.UIWeb.Driver;
using Unicorn.UIWeb.UI;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        IDriver driver;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            driver = WebDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            driver.Find<WebControl>(By.Web_Xpath, "//li[@data-department='Электроника']/a").Click();
            WebControl checkbox = driver.Find<WebControl>(By.Web_Xpath, "//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']");
            checkbox.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
