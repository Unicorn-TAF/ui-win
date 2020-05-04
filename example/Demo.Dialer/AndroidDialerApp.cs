using Demo.AndroidDialer.Ui;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.Driver;
using Unicorn.UI.Mobile.Android.PageObject;

namespace Demo.AndroidDialer
{
    public class AndroidDialerApp : AndroidApplication
    {
        private static AndroidDialerApp _instance = null;

        public AndroidDialerApp(string hubAddress, string deviceName) : base("com.android.dialer", "DialtactsActivity", "4.4.4", deviceName, hubAddress) { }

        public static AndroidDialerApp Instance => _instance ?? 
            (_instance = new AndroidDialerApp("http://127.0.0.1:4723/wd/hub", "device"));

        public DialerFrame App => AndroidAppDriver.Instance.Find<DialerFrame>(ByLocator.Id("android:id/action_bar_overlay_layout"));
    }
}
