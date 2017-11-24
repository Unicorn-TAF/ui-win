using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace ProjectSpecific.Gui
{
    public class CalculatorWindow : Window
    {
        [Find(LocatorType.Id, "num9Button")]
        public Button ButtonNine;
    }
}
