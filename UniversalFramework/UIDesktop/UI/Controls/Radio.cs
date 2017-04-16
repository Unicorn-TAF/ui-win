using System;
using System.Windows.Automation;
using UICore.UI;

namespace UIDesktop.UI.Controls
{
    class Radio : GuiControl, ISelectable
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
