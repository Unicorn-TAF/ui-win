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
    public abstract class Application : WinContainer
    {
        protected Application(string path, string exeName)
        {
            this.SearchContext = WinDriver.Instance.SearchContext;
            ContainerFactory.InitContainer(this);
            this.Path = path;
            this.ExeName = exeName;
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_PaneControlTypeId;

        public string Path { get; protected set; }

        public string ExeName { get; protected set; }

        public Process Process { get; protected set; }

        public virtual void Start()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Start {this.ExeName} application");
            this.Process = Process.Start(System.IO.Path.Combine(this.Path, this.ExeName));
        }

        public virtual void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Close {this.ExeName} application");
            try
            {
                new Window(WinDriver.Driver.ElementFromHandle(this.Process.MainWindowHandle)).Close();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to close {this.ExeName} application: {ex.Message}");
            }
        }
    }
}
