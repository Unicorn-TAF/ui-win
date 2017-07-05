using System;
using System.Windows.Automation;
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
                return Name;
            }
        }
        

        public void Close()
        {
            var pattern = GetPattern<WindowPattern>();
            pattern.Close();
        }
    }
}
