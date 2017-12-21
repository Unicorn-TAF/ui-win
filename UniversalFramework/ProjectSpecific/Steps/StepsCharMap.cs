using AspectInjector.Broker;
using ProjectSpecific.UI.Gui;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsCharMap : TestSteps
    {
        private IDriver driver;
        public WindowCharMap CharMap;

        [TestStep("Start '{0}'")]
        public void StartApplication(string value)
        {
            driver = GuiDriver.Instance;
            driver.Get(value);
            CharMap = driver.Find<WindowCharMap>(ByLocator.Name("Character Map"));
        }

        [TestStep("Select '{0}' font")]
        public void SelectFont(string fontName)
        {
            CharMap.DropdownFonts.Select(fontName);
            CharMap.ButtonHelp.Click();
        }

        [TestStep("Close application")]
        public void CloseApplication()
        {
            driver.Close();
        }
    }
}
