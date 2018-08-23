using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Checkbox : WinControl, ICheckbox
    {
        public Checkbox()
        {
        }

        public Checkbox(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_CheckBoxControlTypeId;

        public bool Checked => this.TogglePattern.CurrentToggleState.Equals(ToggleState.ToggleState_On);

        protected IUIAutomationTogglePattern TogglePattern => this.GetPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;

        public bool Check()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Check {this.ToString()}");
            if (this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to check (checked by default)");
                return false;
            }

            this.TogglePattern.Toggle();
            Logger.Instance.Log(LogLevel.Trace, "\tChecked");

            return true;
        }

        public bool Uncheck()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Uncheck {this.ToString()}");
            if (!this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to uncheck (unchecked by default)");
                return false;
            }

            this.TogglePattern.Toggle();
            Logger.Instance.Log(LogLevel.Trace, "\tUnchecked");

            return true;
        }
    }
}
