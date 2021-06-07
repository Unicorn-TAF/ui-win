using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Mobile.Android.Controls;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    /// <summary>
    /// Extension for AndroidControl
    /// </summary>
    public static class AndroidControlExtension
    {
        /// <summary>
        /// Check if page object exists it the moment ignoring implicitly wait
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
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
