using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Mobile.Android.Controls;

namespace Unicorn.UnitTests.UI.Gui.Android
{
    public class DialerFrame : AndroidContainer
    {
        [ById("com.google.android.dialer:id/dialtacts_mainlayout")]
        public DialPad DialPad { get; set; }

        [ById("com.google.android.dialer:id/lists_pager_header")]
        public DialerActionBar ActionBar { get; set; }

        [ById("com.google.android.dialer:id/floating_action_button")]
        public AndroidControl ButtonDialPad { get; set; }
    }
}
