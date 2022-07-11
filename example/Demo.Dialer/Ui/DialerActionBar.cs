using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    [Find(Using.Id, "com.google.android.dialer:id/lists_pager_header")]
    public class DialerActionBar : AndroidContainer
    {
        [Find(Using.WebXpath, "//android.widget.ImageView[@content-desc='Call History']")]
        public AndroidControl ButtonHistory { get; set; }
    }
}
