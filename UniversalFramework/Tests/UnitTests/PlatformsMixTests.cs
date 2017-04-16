using NUnit.Framework;
using System.Threading;
using UICore.Driver;
using UIDesktop.Driver;
using UIDesktop.UI.Controls;
using UIWeb.Driver;
using UIWeb.UI;

namespace Tests.UnitTests
{

    public class PlatformsMixTests
    {
        IDriver driver;

        const string EXE_PATH = @"D:\SCIENCE\Programs\MathAnalysisSoftware\_Release\";
        const string PORTAL_URL = @"https://devmi3-clients.ileveldev.com/Profile/Login.aspx?ReturnUrl=%2f";

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            driver = GuiDriver.Instance;
            driver.Get(EXE_PATH + "TimeSeriesAnalysis.exe");

            Window mainWindow = driver.WaitForElement<Window>("mainForm");
            mainWindow.ClickButton("openFileBtn");

            Window openDialog = mainWindow.WaitForElement<Window>("Open");
            openDialog.InputText("File name:", EXE_PATH + "TestData\\henon");
            openDialog.ClickButton("Open");
            Thread.Sleep(2000);

            mainWindow.ClickButton("plotBtn");

            driver.Close();

            driver = WebDriver.Instance;
            driver.Get(PORTAL_URL);

            
            driver.WaitForElement<WebControl>(".//input[@id='ctrlLogin_LoginButton']").Click();
            WebControl checkbox = driver.WaitForElement<WebControl>(".//input[@id='ctrlLogin_iLevelRememberMe']");
            checkbox.Click();

            driver.Close();
        }
    }
}
