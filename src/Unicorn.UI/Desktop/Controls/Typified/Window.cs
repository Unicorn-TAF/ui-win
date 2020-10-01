using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base window control.
    /// </summary>
    public class Window : GuiContainer, IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Window(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA window control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Window;

        /// <summary>
        /// Gets window title text.
        /// </summary>
        public virtual string Title => Text;

        /// <summary>
        /// Closes window.
        /// </summary>
        public virtual void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Close {ToString()}");
            Instance.GetPattern<WindowPattern>().Close();
        }

        /// <summary>
        /// Focuses on the window.
        /// </summary>
        public void Focus()
        {
            try
            {
                Logger.Instance.Log(LogLevel.Debug, $"Focusing {ToString()}");
                Instance.SetFocus();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Unable to focus window: {ex.Message}");
            }
        }

        /// <summary>
        /// Wait until window is closed during specified timeout.
        /// </summary>
        /// <param name="timeout">timeout to wait</param>
        public virtual void WaitForClosed(TimeSpan timeout)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Wait for {ToString()} closing");
            var timer = Stopwatch.StartNew();

            var originalTimeout = GuiDriver.ImplicitlyWaitTimeout;
            GuiDriver.ImplicitlyWaitTimeout = TimeSpan.FromSeconds(0);

            try
            {
                do
                {
                    Thread.Sleep(50);
                }
                while (Visible && timer.Elapsed < timeout);
            }
            catch (ControlNotFoundException)
            {
                // positive case, window disappeared
            }

            timer.Stop();

            GuiDriver.ImplicitlyWaitTimeout = originalTimeout;

            if (timer.Elapsed > timeout)
            {
                throw new ControlInvalidStateException("Failed to wait for window is closed!");
            }

            Logger.Instance.Log(LogLevel.Trace, $"Closed. [Wait time = {timer.Elapsed}]");
        }

        /// <summary>
        /// Wait until window is closed during 30 seconds.
        /// </summary>
        public void WaitForClosed() => WaitForClosed(TimeSpan.FromSeconds(30));
    }
}
