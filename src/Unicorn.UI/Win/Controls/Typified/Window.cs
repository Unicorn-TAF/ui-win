using System;
using System.Diagnostics;
using System.Threading;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Window : WinContainer, IWindow
    {
        public Window()
        {
        }

        public Window(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_WindowControlTypeId;

        public string Title => this.Text;

        protected IUIAutomationWindowPattern WindowPattern => this.GetPattern(UIA_PatternIds.UIA_WindowPatternId) as IUIAutomationWindowPattern;

        public virtual void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Close {this.ToString()}");
            WindowPattern.Close();
        }

        public void Focus()
        {
            try
            {
                Logger.Instance.Log(LogLevel.Debug, $"Focusing {this.ToString()}");
                Instance.SetFocus();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to focus window: {ex.Message}");
            }
        }

        public void WaitForClosed(int timeout = 5000)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Wait for {this.ToString()} closing");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            var originalTimeout = WinDriver.ImplicitlyWaitTimeout;
            WinDriver.ImplicitlyWaitTimeout = TimeSpan.FromSeconds(0);

            try
            {
                do
                {
                    Thread.Sleep(50);
                }
                while (this.Visible && timer.ElapsedMilliseconds < timeout);
            }
            catch (ControlNotFoundException)
            {
                // Window not found, wait is successful
            }

            timer.Stop();

            WinDriver.ImplicitlyWaitTimeout = originalTimeout;

            if (timer.ElapsedMilliseconds > timeout)
            {
                throw new ControlInvalidStateException("Failed to wait for window is closed!");
            }

            Logger.Instance.Log(LogLevel.Trace, $"Closed. [Wait time = {timer.Elapsed}]");
        }
    }
}
