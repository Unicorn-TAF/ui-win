using System;
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

        public bool SetCheckState(bool isChecked)
        {
            return isChecked ? Check() : Uncheck();
        }

        private bool Check()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Check {this.ToString()}");
            if (this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to check (checked by default)");
                return false;
            }

            GetPattern<TogglePattern>().Toggle();
            Logger.Instance.Log(LogLevel.Trace, "\tChecked");

            return true;
        }

        private bool Uncheck()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Uncheck {this.ToString()}");
            if (!this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to uncheck (unchecked by default)");
                return false;
            }

            GetPattern<TogglePattern>().Toggle();
            Logger.Instance.Log(LogLevel.Trace, "\tUnchecked");

            return true;
        }
    }
}
