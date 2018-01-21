using AspectInjector.Broker;
using ProjectSpecific.UI;
using ProjectSpecific.UI.Gui;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Desktop.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsCharMap : TestSteps
    {
        private WindowCharMap CharMap => Desktop.CharMap;

        [TestStep("Start '{0}'")]
        public void StartApplication(string value)
        {
            GuiDriver.Instance.Get(value);
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
            CharMap.Close();
        }
    }
}
