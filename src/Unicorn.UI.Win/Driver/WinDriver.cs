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
        private static WinDriver instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinDriver"/> with default implicit timeout.
        /// </summary>
        private WinDriver()
        {
            if (UseIUia2)
            {
                Driver = new CUIAutomation8();
            }
            else
            {
                Driver = new CUIAutomation();
            }
            
            SearchContext = Driver.GetRootElement();
            ImplicitlyWait = TimeoutDefault;
            Logger.Instance.Log(LogLevel.Debug, "UI Automation Driver initialized");
        }

        /// <summary>
        /// Gets or sets value indicating whether to use IUIAutomation2 interface or not (default: false)<br/>
        /// IUIAutomation2 is documented to work on Win 8 and higher.
        /// </summary>
        public static bool UseIUia2 { get; set; } = false;

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
                    instance = new WinDriver();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets IUIAutomation instance.<br/>
        /// if IUIAutomation2 is used it's possible to set timeouts (50 is min value):<br/>
        /// - (Driver as CUIAutomation8).ConnectionTimeout<br/>
        /// - (Driver as CUIAutomation8).TransactionTimeout
        /// </summary>
        public IUIAutomation Driver { get; }

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

        /// <summary>
        /// Closes Win driver instance.
        /// </summary>
        public static void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, "Close Win driver");
            instance = null;
        }
    }
}
