using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Radio : GuiControl, ISelectable
    {
        public Radio()
        {
        }

        public Radio(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.RadioButton;

        public virtual bool Selected =>
            (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected;

        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            var pattern = this.Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Trace, "Selected");

            return true;
        }
    }
}
