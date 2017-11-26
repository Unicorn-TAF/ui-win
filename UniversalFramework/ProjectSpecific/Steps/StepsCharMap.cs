using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Driver;
using ProjectSpecific.UI.Gui;

namespace ProjectSpecific.Steps
{
    public class StepsCharMap : TestSteps
    {
        IDriver driver;


        [TestStep("Start '{0}'")]
        public void StartApplication(string value)
        {
            ReportStep(value);

            driver = GuiDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Select '{0}' font")]
        public void DoSomething(string fontName)
        {
            ReportStep(fontName);

            WindowCharMap charMap = driver.Find<WindowCharMap>(ByLocator.Name("Character Map"));
            charMap.DropdownFonts.Select(fontName);

            charMap.ButtonHelp.Click();
        }

        [TestStep("Close application")]
        public void CloseApplication()
        {
            ReportStep();

            driver.Close();
        }
    }
}
