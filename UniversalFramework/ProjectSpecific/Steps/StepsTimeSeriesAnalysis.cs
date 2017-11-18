using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using System.Threading;
using Unicorn.UICore.Driver;
using Unicorn.UIDesktop.Driver;
using Unicorn.UIDesktop.UI.Controls;
using AspectInjector.Broker;
using System.Reflection;
using System.Diagnostics;

namespace ProjectSpecific.Steps
{
    //[Aspect(typeof(TestStepsEvents),AccessModifierFilter =AccessModifiers.Public)]
    public class StepsTimeSeriesAnalysis : TestSteps
    {
        IDriver driver;


        [TestStep("Start Time Series Analysis '{0}'")]
        public void StartApplication(string value)
        {
            driver = GuiDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Open File '{0}' For Analysis")]
        public void OpenFile(string fileName)
        {
            Window mainWindow = driver.Find<Window>(By.Id, "mainForm");
            mainWindow.ClickButton("openFileBtn");

            Window openDialog = mainWindow.Find<Window>("Open");
            openDialog.InputText("File name:", fileName);
            openDialog.ClickButton("Open");
            Thread.Sleep(2000);
        }



        [TestStep("Draw charts")]
        public void DrawCharts()
        {
            Window mainWindow = driver.Find<Window>(By.Name, "mainForm");
            mainWindow.ClickButton("plotBtn");
        }


        [TestStep("Close Time Series Analysis")]
        public void CloseApplication()
        {
            driver.Close();
        }
    }

}
