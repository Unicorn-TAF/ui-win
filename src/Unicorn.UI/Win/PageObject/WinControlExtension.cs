using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.PageObject
{
    public static class WinControlExtension
    {
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
