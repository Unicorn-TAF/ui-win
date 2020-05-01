using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    public class DialerFrame : AndroidContainer
    {
        [Find(Using.Id, "com.android.dialer:id/top")]
        public DialPad MainFrame { get; set; }

        [Find(Using.Id, "com.android.dialer:id/fake_action_bar")]
        public DialerActionBar ActionBar { get; set; }

        [Find(Using.Id, "com.android.dialer:id/dialpad_button")]
        public AndroidControl ButtonDialPad { get; set; }
    }
}
