using System.Threading;
using System.Windows.Automation;
using Unicorn.UICore.UI;
using Unicorn.UICore.UI.Controls;

namespace Unicorn.UIDesktop.UI.Controls
{
    public class Window : GuiContainer, IWindow
    {
        public override ControlType Type { get { return ControlType.Window; } }

        public Window() { }

        public Window(AutomationElement instance)
            : base(instance)
        {
        }

        public string Title
        {
            get
            {
                return Text;
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
            catch { }
        }

        public void WaitForClosed(int timeout = 5000)
        {
            do
            {
                Thread.Sleep(100);
                timeout -= 100;
            } while (Visible && timeout > 0);
            if (timeout <= 0)
                throw new ControlInvalidStateException("Failed to wait for window is closed!");
        }
    }
}
