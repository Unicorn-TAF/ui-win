using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UnitTests.UI.Gui.Web
{
    [Name(ControlName)]
    [Find(Using.WebCss, ".ui-checkboxradio-label")]
    public class CustomCheckbox : WebControl
    {
        internal const string ControlName = "Custom checkbox";

        [Find(Using.WebCss, ".ui-checkboxradio-icon")]
        private readonly WebControl _label;

        public WebControl Label => _label;
    }
}
