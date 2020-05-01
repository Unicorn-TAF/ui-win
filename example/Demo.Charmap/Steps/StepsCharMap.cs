using AspectInjector.Broker;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.Charmap.Steps
{
    [Inject(typeof(StepsEvents))]
    public class StepsCharMap
    {
        CharmapApp Charmap => CharmapApp.Charmap;

        [Step("Start charmap")]
        public void StartApplication() =>
            Charmap.Start();

        [Step("Select '{0}' font")]
        public void SelectFont(string fontName) =>
            Charmap.Window.DropdownFonts.Select(fontName);

        [Step("Select current symbol")]
        public void SelectCurrentSymbol() =>
            Charmap.Window.ButtonSelect.Click();

        [Step("Set 'Advanced view' to '{0}'")]
        public void SetAdvancedView(bool on) =>
            Charmap.Window.SetCheckbox("Advanced view", on);

        [Step("Close application")]
        public void CloseApplication() =>
            Charmap.Close();
    }
}
