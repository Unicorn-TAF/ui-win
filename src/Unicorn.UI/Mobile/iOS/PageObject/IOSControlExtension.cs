using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Mobile.IOS.Controls;
using Unicorn.UI.Mobile.IOS.Driver;

namespace Unicorn.UI.Mobile.iOS.PageObject
{
    public static class IOSControlExtension
    {
        public static bool Exists(this IOSControl control)
        {
            var originalTimeout = IOSDriver.Instance.ImplicitlyWait;
            IOSDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

            try
            {
                control.Instance.GetType();
                return true;
            }
            catch (ControlNotFoundException)
            {
                return false;
            }
            finally
            {
                IOSDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
