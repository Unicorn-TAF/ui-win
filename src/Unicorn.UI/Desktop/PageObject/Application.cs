using System;
using System.Diagnostics;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Controls.Typified;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.PageObject
{
    public abstract class Application : GuiContainer
    {
        protected Application(string path, string exeName)
        {
            this.SearchContext = GuiDriver.Instance.SearchContext;
            ContainerFactory.InitContainer(this);
            this.Path = path;
            this.ExeName = exeName;
        }

        public override ControlType Type => ControlType.Pane;

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
                new Window(AutomationElement.FromHandle(this.Process.MainWindowHandle)).Close();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to close {this.ExeName} application: {ex.Message}");
            }
        }
    }
}
