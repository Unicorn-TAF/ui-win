using AspectInjector.Broker;
using ProjectSpecific.UI;
using ProjectSpecific.UI.Web;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Web.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        private PageYandex Yandex => Pages.YandexMarket;

        [TestStep("Navigate to '{0}'")]
        public void NavigateTo(string value)
        {
            WebDriver.Instance.Get(value);
        }

        [TestStep("Select 'Electronics' catalog")]
        public void SelectCatalog()
        {
            Yandex.MenuTop.LinkElectronics.Click();
        }

        [TestStep("Select 'Mobile phones' sub-catalog")]
        public void SelectSubCatalog()
        {
            Yandex.LinkMobilePhones.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            WebDriver.Instance.Close();
        }
    }
}
