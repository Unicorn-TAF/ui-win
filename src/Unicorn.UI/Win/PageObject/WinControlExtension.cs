using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.PageObject
{
    /// <summary>
    /// Extension for <see cref="WinControl"/>
    /// </summary>
    public static class WinControlExtension
    {
        /// <summary>
        /// Check if page object exists it the moment ignoring implicitly wait
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool Exists(this WinControl control)
        {
            var originalTimeout = WinDriver.Instance.ImplicitlyWait;
            WinDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

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
                WinDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
