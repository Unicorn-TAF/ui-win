using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.PageObject
{
    /// <summary>
    /// Extension for <see cref="GuiControl"/>
    /// </summary>
    public static class GuiControlExtension
    {
        /// <summary>
        /// Check if page object exists it the moment ignoring implicitly wait
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool Exists(this GuiControl control)
        {
            var originalTimeout = GuiDriver.Instance.ImplicitlyWait;
            GuiDriver.Instance.ImplicitlyWait = TimeSpan.FromSeconds(0);

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
                GuiDriver.Instance.ImplicitlyWait = originalTimeout;
            }
        }
    }
}
