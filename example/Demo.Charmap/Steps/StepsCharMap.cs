//using AspectInjector.Broker;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.Charmap.Steps
{
    /// <summary>
    /// Represents high-level steps for application.
    /// To make steps be able to use events subscriptions it's necessary 
    /// to use <see cref="Inject"/> with <see cref="StepsEvents"/>.
    /// </summary>
    //[Inject(typeof(StepsEvents))]
    public class StepsCharMap
    {
        CharmapApp Charmap => CharmapApp.Charmap;

        /// <summary>
        /// Example of step with description (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// </summary>
        [Step("Start charmap")]
        public void StartApplication() =>
            Charmap.Start();

        /// <summary>
        /// Example of step with description and parmeters (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// With placeholders parameters could be substitured into description.
        /// </summary>
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
