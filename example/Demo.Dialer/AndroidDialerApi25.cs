using Demo.AndroidDialer.Ui;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.PageObject;

namespace Demo.AndroidDialer
{
    /// <summary>
    /// Represents dialer app for android (for API v.25). Inherits <see cref="AndroidApplication"/> 
    /// to support base android app functionality.
    /// </summary>
    public class AndroidDialerApi25 : AndroidApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDialerApi25"/> class with appium server 
        /// url and device name. Calls base constructor with app package, activity and platform version.
        /// </summary>
        /// <param name="hubAddress">address of appium server</param>
        /// <param name="deviceName">name of device</param>
        public AndroidDialerApi25(string hubAddress, string deviceName) 
            : base("com.google.android.dialer", "DialtactsActivity", "7.1.1", deviceName, hubAddress) 
        { 
        }

        /// <summary>
        /// Gets main application frame as search context for child controls.
        /// </summary>
        public DialerFrame AppFrame => 
            Driver.Find<DialerFrame>(ByLocator.Id("com.google.android.dialer:id/decor_content_parent"));
    }
}
