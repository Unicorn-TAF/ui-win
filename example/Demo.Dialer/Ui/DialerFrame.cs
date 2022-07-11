using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    public class DialerFrame : AndroidContainer
    {
        public DialPad DialPad { get; set; }

        public DialerActionBar ActionBar { get; set; }

        [ById("com.google.android.dialer:id/floating_action_button")]
        public AndroidControl DialPadButton { get; set; }

        public CallsHistoryPane CallsHistory { get; set; }
    }
}
