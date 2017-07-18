using Unicorn.Core.Testing;
using Unicorn.Core.Testing.Attributes;
using Unicorn.UICore.Driver;
using Unicorn.UIWeb.Driver;
using Unicorn.UIWeb.UI;

namespace ProjectSpecific.Steps
{
    public class WebSteps : TestSteps
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

            driver.Find<WebControl>(By.Web_Xpath, ".//input[@id='ctrlLogin_LoginButton']").Click();
            WebControl checkbox = driver.Find<WebControl>(By.Web_Xpath, ".//input[@id='ctrlLogin_iLevelRememberMe']");
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
