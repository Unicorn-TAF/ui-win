using System;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Driver
{
    /// <summary>
    /// Represents Driver for Windows GUI and allows to perform search of elements in UI Automation tree.
    /// </summary>
    public class GuiDriver : GuiSearchContext, IDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiDriver"/> with default implicit timeout.
        /// </summary>
        private GuiDriver()
        {
            SearchContext = AutomationElement.RootElement;
            ImplicitlyWait = TimeoutDefault;
            Logger.Instance.Log(LogLevel.Debug, "UI Automation Driver initialized");
        }

        /// <summary>
        /// Gets instance of Desktop driver.
        /// Initialized with default implicitly wait timeout.
        /// </summary>
        public static GuiDriver Instance { get; } = new GuiDriver();

        /// <summary>
        /// Gets or sets implicit timeout of waiting for specified element to be existed in elements tree.
        /// </summary>
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
