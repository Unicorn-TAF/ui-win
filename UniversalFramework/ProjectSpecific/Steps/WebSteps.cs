using Unicorn.Core.Testing;
using Unicorn.Core.Testing.Attributes;
using Unicorn.UICore.Driver;
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

            driver = Web.Instance;
            driver.Get(value);
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            ReportStep();

            driver.FindControl<WebControl>(By.Web_Xpath, ".//input[@id='ctrlLogin_LoginButton']").Click();
            WebControl checkbox = driver.FindControl<WebControl>(By.Web_Xpath, ".//input[@id='ctrlLogin_iLevelRememberMe']");
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
