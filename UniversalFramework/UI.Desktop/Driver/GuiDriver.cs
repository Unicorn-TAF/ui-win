using System;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Driver
{
    public class GuiDriver : GuiSearchContext, IDriver
    {
        private static GuiDriver instance = null;

        public static GuiDriver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiDriver();
                    instance.SearchContext = AutomationElement.RootElement;
                    instance.ImplicitlyWait = instance.TimeoutDefault;
                    Logger.Instance.Log(LogLevel.Debug, "UI Automation Driver initialized");
                }

                return instance;
            }
        }

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return GuiSearchContext.ImplicitlyWaitTimeout;
            }

            set
            {
                GuiSearchContext.ImplicitlyWaitTimeout = value;
            }
        }
    }
}
