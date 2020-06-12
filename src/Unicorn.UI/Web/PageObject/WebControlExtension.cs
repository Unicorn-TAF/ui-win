using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.PageObject
{
    /// <summary>
    /// Extension for <see cref="WebControl"/>
    /// </summary>
    public static class WebControlExtension
    {
        /// <summary>
        /// Check if page object exists it the moment ignoring implicitly wait
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool Exists(this WebControl control)
        {
            var originalTimeout = WebDriver.Instance.ImplicitlyWait;
            WebDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

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
                WebDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
