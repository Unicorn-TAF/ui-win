using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Mobile.Android.Controls;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public static class AndroidControlExtension
    {
        public static bool Exists(this AndroidControl control)
        {
            var originalTimeout = AndroidAppDriver.Instance.ImplicitlyWait;
            AndroidAppDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

            try
            {
                return control.Instance.GetType() != null;
            }
            catch (ControlNotFoundException)
            {
                return false;
            }
            finally
            {
                AndroidAppDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
