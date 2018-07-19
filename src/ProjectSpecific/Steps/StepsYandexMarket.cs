using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket
    {
        [TestStep("Open Yandex Market")]
        public void Open()
        {
            TestEnvironment.Instance.YandexMarket.Open();
        }

        [TestStep("Select 'Electronics' catalog")]
        public void SelectCatalog()
        {
            TestEnvironment.Instance.YandexMarket.MainPage.MenuTop.LinkElectronics.Click();
        }

        [TestStep("Select 'Mobile phones' sub-catalog")]
        public void SelectSubCatalog()
        {
            TestEnvironment.Instance.YandexMarket.MainPage.LinkMobilePhones.Click();

            TestEnvironment.Instance.YandexMarket.MainPage.MenuTop.LinkElectronics
                .Wait(Until.AttributeContains, "class", "topmenu__item_mode_current")
                .Wait(Until.Visible);
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            WebDriver.Close();
        }
    }
}
