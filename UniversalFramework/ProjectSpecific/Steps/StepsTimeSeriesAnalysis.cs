using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Driver;
using ProjectSpecific.Gui;
using Unicorn.UI.Desktop.Controls.Typified;

namespace ProjectSpecific.Steps
{
    public class StepsTimeSeriesAnalysis : TestSteps
    {
        IDriver driver;


        [TestStep("Start Time Series Analysis '{0}'")]
        public void StartApplication(string value)
        {
            ReportStep(value);

            driver = GuiDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Open File '{0}' For Analysis")]
        public void OpenFile(string fileName)
        {
            ReportStep(fileName);

            //Window mainWindow = driver.Find<Window>(ByLocator.Name("Signal Analyzer"));
            //mainWindow.Find<Button>(ByLocator.Id("openFileBtn"));

            //Window openDialog = mainWindow.Find<Window>(ByLocator.Name("Open"));
            //openDialog.InputText("File name:", fileName);
            //openDialog.ClickButton("Open");
            //Thread.Sleep(2000);

            CalculatorWindow mainWindow = driver.Find<CalculatorWindow>(ByLocator.Name("Calculator"));
            mainWindow.ButtonNine.Click();
        }



        [TestStep("Draw charts")]
        public void DrawCharts()
        {
            ReportStep();

            Window mainWindow = driver.Find<Window>(ByLocator.Name("mainForm"));
            mainWindow.ClickButton("plotBtn");
        }


        [TestStep("Close Time Series Analysis")]
        public void CloseApplication()
        {
            ReportStep();

            driver.Close();
        }
    }
}
