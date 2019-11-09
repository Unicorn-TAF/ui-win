using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Mobile.Ios.Controls;
using Unicorn.UI.Mobile.Ios.Driver;

namespace Unicorn.UI.Mobile.Ios.PageObject
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
        public static bool Exists(this IosControl control)
        {
            var originalTimeout = IosDriver.Instance.ImplicitlyWait;
            IosDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

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
                IosDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
