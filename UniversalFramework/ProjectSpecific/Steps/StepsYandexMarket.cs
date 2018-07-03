using AspectInjector.Broker;
using ProjectSpecific.UI.Web;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Web.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        [TestStep("Open Yandex Market")]
        public void Open()
        {
            TestEnvironment.Instance.YandexMarket.NavigateTo<PageYandexMarketMain>();
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
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            WebDriver.Close();
        }
    }
}
