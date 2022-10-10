using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    /// <summary>
    /// Describes base checkbox control.
    /// </summary>
    public class Checkbox : WebControl, ICheckbox
    {
        /// <summary>
        /// Gets a value indicating whether checkbox is checked.
        /// </summary>
        public virtual bool Checked => Instance.Selected;

        /// <summary>
        /// Sets checkbox checked state
        /// </summary>
        /// <param name="isChecked">true - to check; false - to uncheck</param>
        /// <returns>true - is state was changed; false - if already in specified state</returns>
        public virtual bool SetCheckedState(bool isChecked) =>
            isChecked ? Check() : Uncheck();

        private bool Check()
        {
            ULog.Debug("Check {0}", this);

            if (Checked)
            {
                ULog.Trace("No need to check (checked by default)");
                return false;
            }

            Click();

            ULog.Trace("Checked");

            return true;
        }

        private bool Uncheck()
        {
            ULog.Debug("Uncheck {0}", this);

            if (!Checked)
            {
                ULog.Trace("No need to uncheck (unchecked by default)");
                return false;
            }

            Click();
            ULog.Trace("Unchecked");

            return true;
        }
    }
}
