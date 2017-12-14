using AspectInjector.Broker;
using ProjectSpecific.UI.Web;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        private IDriver driver;
        private PageYandex yandex;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            driver = WebDriver.Instance;
            driver.Get(value);
            yandex = new PageYandex();
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            yandex.MenuTop.LinkElectronics.Click();
            yandex.LinkMobilePhones.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
