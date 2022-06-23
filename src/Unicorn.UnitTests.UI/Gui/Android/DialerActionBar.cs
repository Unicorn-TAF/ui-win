using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Controls;

namespace Unicorn.UnitTests.UI.Gui.Android
{
    public class DialerActionBar : AndroidControl
    {
        [Find(Using.WebXpath, "//android.widget.ImageView[@content-desc='Call History']")]
        public AndroidControl ButtonHistory { get; set; }
    }
}
