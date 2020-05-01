using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    public class DialerActionBar : AndroidContainer
    {
        [Find(Using.Id, "com.android.dialer:id/call_history_button")]
        public AndroidControl ButtonHistory { get; set; }

        [Find(Using.Id, "com.android.dialer:id/dial_button")]
        public AndroidControl ButtonDial { get; set; }

        [Find(Using.Id, "com.android.dialer:id/overflow_menu")]
        public AndroidControl ButtonMenu { get; set; }
    }
}
