using System;
using System.Diagnostics;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UI.Desktop.Driver
{
    public class GuiDriver : GuiSearchContext, IDriver
    {
        private static GuiDriver instance = null;
        private Process currentProcess;

        public static GuiDriver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiDriver();
                    instance.SearchContext = AutomationElement.RootElement;
                    Logger.Instance.Debug("UI Automation Driver initialized");
                }

                return instance;
            }
        }

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return this.ImplicitlyWaitTimeout;
            }

            set
            {
                this.ImplicitlyWaitTimeout = value;
            }
        }

        public void Close()
        {
            try
            {
                new Window(AutomationElement.FromHandle(this.currentProcess.MainWindowHandle)).Close();
            }
            catch
            {
            }
        }

        public void Get(string path)
        {
            this.currentProcess = Process.Start(path);
        }
    }
}
