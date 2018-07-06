using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsCharMap : TestSteps
    {
        [TestStep("Start charmap")]
        public void StartApplication()
        {
            TestEnvironment.Instance.Charmap.Start();
        }

        [TestStep("Select '{0}' font")]
        public void SelectFont(string fontName)
        {
            TestEnvironment.Instance.Charmap.Window.DropdownFonts.Select(fontName);
            TestEnvironment.Instance.Charmap.Window.ButtonHelp.Click();
        }

        [TestStep("Close application")]
        public void CloseApplication()
        {
            TestEnvironment.Instance.Charmap.Close();
        }
    }
}
