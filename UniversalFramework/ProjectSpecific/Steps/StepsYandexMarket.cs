using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UIWeb.Driver;
using Unicorn.UIWeb.UI;

namespace ProjectSpecific.Steps
{
    public class StepsYandexMarket : TestSteps
    {
        IDriver driver;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            ReportStep(value);

            driver = WebDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            ReportStep();

            driver.Find<WebControl>(ByLocator.Xpath("//li[@data-department='Электроника']/a")).Click();
            WebControl checkbox = driver.Find<WebControl>(ByLocator.Xpath("//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']"));
            checkbox.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            ReportStep();

            driver.Close();
        }
    }
}
