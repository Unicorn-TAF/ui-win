using Core.Testing;
using Core.Testing.Attributes;
using System.Reflection;
using UICore.Driver;
using UIWeb.Driver;
using UIWeb.UI;

namespace ProjectSpecific.Steps
{
    public class WebSteps : TestSteps
    {
        IDriver driver;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            ReportMethod(MethodBase.GetCurrentMethod(), value);
            //ReportMethod(value);

            driver = WebDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            ReportMethod(MethodBase.GetCurrentMethod());
            //ReportMethod();

            driver.WaitForElement<WebControl>(".//input[@id='ctrlLogin_LoginButton']").Click();
            WebControl checkbox = driver.WaitForElement<WebControl>(".//input[@id='ctrlLogin_iLevelRememberMe']");
            checkbox.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            ReportMethod(MethodBase.GetCurrentMethod());
            //ReportMethod();

            driver.Close();
        }
    }
}
