using System;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Driver
{
    /// <summary>
    /// Represents Driver for Windows GUI and allows to perform search of elements in UI Automation tree.
    /// </summary>
    public class WinDriver : WinSearchContext, IDriver
    {
        private static WinDriver instance = null;

        /// <summary>
        /// Gets instance of Windows driver.
        /// Initialized with default implicitly wait timeout.
        /// </summary>
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

        /// <summary>
        /// UI Automation driver instance.
        /// </summary>
        public static CUIAutomation Driver { get; set; }

        /// <summary>
        /// Gets or sets implicit timeout of waiting for specified element to be existed in elements tree.
        /// </summary>
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
