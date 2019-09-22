using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class Checkbox : WebControl, ICheckbox
    {
        public virtual bool Checked => this.Instance.Selected;

        public virtual bool SetCheckedState(bool isChecked) =>
            isChecked ? Check() : Uncheck();

        private bool Check()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Check {this.ToString()}");

            if (this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to check (checked by default)");
                return false;
            }

            this.Click();

            Logger.Instance.Log(LogLevel.Trace, "Checked");

            return true;
        }

        private bool Uncheck()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Uncheck {this.ToString()}");

            if (!this.Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to uncheck (unchecked by default)");
                return false;
            }

            this.Click();
            Logger.Instance.Log(LogLevel.Trace, "Unchecked");

            return true;
        }
    }
}
