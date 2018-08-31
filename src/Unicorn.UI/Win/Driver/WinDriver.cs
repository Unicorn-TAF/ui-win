using System;
using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Driver
{
    public class WinDriver : WinSearchContext, IDriver
    {
        private static WinDriver instance = null;

        public static WinDriver Instance
        {
            get
            {
                if (instance == null)
                {
                    Driver = new CUIAutomation();
                    instance = new WinDriver();
                    instance.SearchContext = Driver.GetRootElement();
                    instance.ImplicitlyWait = instance.TimeoutDefault;
                    Logger.Instance.Log(LogLevel.Debug, "UI Automation Driver initialized");
                }

                return instance;
            }
        }

        public static CUIAutomation Driver { get; set; }

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return WinSearchContext.ImplicitlyWaitTimeout;
            }

            set
            {
                WinSearchContext.ImplicitlyWaitTimeout = value;
            }
        }
    }
}
