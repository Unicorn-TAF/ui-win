using System;
using System.Diagnostics;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.PageObject
{
    /// <summary>
    /// Represents base of windows application. Contains fields related to paths, process, methods for starting and closing the application.
    /// </summary>
    public abstract class Application : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class located in specified directory and with specified exe name.
        /// </summary>
        /// <param name="path">path to application</param>
        /// <param name="exeName">.exe file name</param>
        protected Application(string path, string exeName)
        {
            SearchContext = WinDriver.Instance.SearchContext;
            ContainerFactory.InitContainer(this);
            Path = path;
            ExeName = exeName;
        }

        /// <summary>
        /// Gets app root control type (by default it's Pane as root of windows desktop).
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_PaneControlTypeId;

        /// <summary>
        /// Gets or sets path to application.
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Gets or sets application exe file name.
        /// </summary>
        public string ExeName { get; protected set; }

        /// <summary>
        /// Gets or sets Process of application instance.
        /// </summary>
        public Process Process { get; protected set; }

        /// <summary>
        /// Start application and assign process to started instance.
        /// </summary>
        public virtual void Start()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Start {ExeName} application");
            Process = Process.Start(System.IO.Path.Combine(Path, ExeName));
        }

        /// <summary>
        /// Close opened application instance.
        /// </summary>
        public virtual void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Close {ExeName} application");
            try
            {
                new Window(WinDriver.Instance.Driver.ElementFromHandle(Process.MainWindowHandle)).Close();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to close {ExeName} application: {ex.Message}");
            }
        }
    }
}
