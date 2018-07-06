using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Radio : GuiControl
    {
        public Radio()
        {
        }

        public Radio(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.RadioButton;

        public bool IsSelected
        {
            get
            {
                return (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected;
            }
        }

        public bool Select()
        {
            SelectionItemPattern pattern = Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            return true;
        }
    }
}
