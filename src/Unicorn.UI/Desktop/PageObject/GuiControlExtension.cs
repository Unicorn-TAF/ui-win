using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.PageObject
{
    public static class GuiControlExtension
    {
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
