using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.PageObject;

namespace Unicorn.UnitTests.UI.Gui.Android
{
    public class AndroidDialerApi25 : AndroidApplication
    {
        public AndroidDialerApi25(string hubAddress, string deviceName) 
            : base("com.google.android.dialer", "DialtactsActivity", "7.1.1", deviceName, hubAddress) 
        {
            
        }

        public DialerFrame Container => 
            Driver.Find<DialerFrame>(ByLocator.Id("com.google.android.dialer:id/decor_content_parent"));
    }
}
