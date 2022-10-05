using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base checkbox control.
    /// </summary>
    public class Checkbox : WinControl, ICheckbox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkbox"/> class.
        /// </summary>
        public Checkbox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Checkbox"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Checkbox(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA checkbox control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_CheckBoxControlTypeId;

        /// <summary>
        /// Gets a value indicating whether checkbox is checked.
        /// </summary>
        public virtual bool Checked => 
            TogglePattern
            .CurrentToggleState
            .Equals(ToggleState.ToggleState_On);

        /// <summary>
        /// Gets toggle patterns instance
        /// </summary>
        protected IUIAutomationTogglePattern TogglePattern => 
            Instance.GetPattern<IUIAutomationTogglePattern>();

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

            TogglePattern.Toggle();
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

            TogglePattern.Toggle();
            ULog.Trace("Unchecked");

            return true;
        }
    }
}
