using System.Windows.Automation;
using Unicorn.Core.Logging;

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
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.IsSelected)
            {
                Logger.Instance.Log(LogLevel.Debug, "\tNo need to select (selected by default)");
                return false;
            }

            var pattern = this.Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Debug, "\tSelected");

            return true;
        }
    }
}
