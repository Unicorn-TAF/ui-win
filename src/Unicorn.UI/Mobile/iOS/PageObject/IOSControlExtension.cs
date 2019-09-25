using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Mobile.IOS.Controls;
using Unicorn.UI.Mobile.IOS.Driver;

namespace Unicorn.UI.Mobile.IOS.PageObject
{
    /// <summary>
    /// Extension for iOS Control
    /// </summary>
    public static class IosControlExtension
    {
        /// <summary>
        /// Check if page object exists it the moment ignoring implicitly wait
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool Exists(this IOSControl control)
        {
            var originalTimeout = IOSDriver.Instance.ImplicitlyWait;
            IOSDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

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
                IOSDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
