using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Window : GuiContainer, IWindow
    {
        public Window()
        {
        }

        public Window(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.Window;

        public string Title
        {
            get
            {
                return this.Text;
            }
        }

        public void Close()
        {
            var pattern = GetPattern<WindowPattern>();
            pattern.Close();
        }

        public virtual void Focus()
        {
            try
            {
                Instance.SetFocus();
            }
            catch
            {
            }
        }

        public void WaitForClosed(int timeout = 5000)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            GuiDriver.ImplicitlyWaitTimeout = TimeSpan.FromSeconds(0);

            try
            {
                do
                {
                    Thread.Sleep(50);
                }
                while (this.Visible && timer.ElapsedMilliseconds < timeout);
            }
            catch (ControlNotFoundException ex)
            {
                timer.Stop();
            }

            GuiDriver.ImplicitlyWaitTimeout = TimeoutDefault;

            if (timer.ElapsedMilliseconds > timeout)
            {
                throw new ControlInvalidStateException("Failed to wait for window is closed!");
            }
        }
    }
}
