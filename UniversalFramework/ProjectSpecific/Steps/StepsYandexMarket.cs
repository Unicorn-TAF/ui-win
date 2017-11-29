using ProjectSpecific.UI.Web;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.Controls;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        IDriver driver;

        public PageYandex Yandex;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            driver = WebDriver.Instance;
            driver.Get(value);
            Yandex = new PageYandex();
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            Yandex.MenuTop.LinkElectronics.Click();
            Yandex.LinkMobilePhones.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
