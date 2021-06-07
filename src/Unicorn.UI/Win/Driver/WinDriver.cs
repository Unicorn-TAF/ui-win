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
        /// <summary>
        /// Initializes a new instance of the <see cref="WinDriver"/> with default implicit timeout.
        /// </summary>
        private WinDriver()
        {
            Driver = new CUIAutomation();
            SearchContext = Driver.GetRootElement();
            ImplicitlyWait = TimeoutDefault;
            Logger.Instance.Log(LogLevel.Debug, "UI Automation Driver initialized");
        }

        /// <summary>
        /// Gets instance of Windows driver.
        /// Initialized with default implicitly wait timeout.
        /// </summary>
        public static WinDriver Instance { get; } = new WinDriver();

        /// <summary>
        /// UI Automation driver instance.
        /// </summary>
        public CUIAutomation Driver { get; set; }

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
