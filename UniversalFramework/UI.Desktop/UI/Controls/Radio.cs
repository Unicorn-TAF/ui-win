using System;
using System.Windows.Automation;
using Unicorn.UI.Core.UI;

namespace Unicorn.UI.Desktop.UI.Controls
{
    public class Radio : GuiControl
    {
        public override ControlType Type { get { return ControlType.RadioButton; } }

        public bool IsSelected
        {
            get
            {
                return (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected;
            }
        }

        public Radio() { }

        public Radio(AutomationElement instance)
            : base(instance)
        {
        }

        public bool Select()
        {
            SelectionItemPattern pattern = Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            return true;
        }
    }
}
