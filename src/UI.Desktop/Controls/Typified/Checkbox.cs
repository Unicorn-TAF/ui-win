using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Checkbox : GuiControl, ICheckbox
    {
        public Checkbox()
        {
        }

        public Checkbox(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.CheckBox;

        public bool Checked => GetPattern<TogglePattern>().Current.ToggleState == ToggleState.On;

        public bool Check()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Check {this.ToString()}");
            if (this.Checked)
            {
                Logger.Instance.Log(LogLevel.Debug, "\tNo need to check (checked by default)");
                return false;
            }

            var pattern = GetPattern<TogglePattern>();
            this.Toggle(pattern);
            Logger.Instance.Log(LogLevel.Debug, "\tChecked");

            return true;
        }

        public bool Uncheck()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Uncheck {this.ToString()}");
            if (!this.Checked)
            {
                Logger.Instance.Log(LogLevel.Debug, "\tNo need to uncheck (unchecked by default)");
                return false;
            }

            var pattern = GetPattern<TogglePattern>();
            this.Toggle(pattern);
            Logger.Instance.Log(LogLevel.Debug, "\tUnchecked");

            return true;
        }

        private void Toggle(TogglePattern pattern)
        {
            pattern.Toggle();
        }
    }
}
