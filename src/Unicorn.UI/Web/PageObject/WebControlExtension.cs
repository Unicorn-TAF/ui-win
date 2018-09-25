using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Desktop.PageObject
{
    public static class WebControlExtension
    {
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
