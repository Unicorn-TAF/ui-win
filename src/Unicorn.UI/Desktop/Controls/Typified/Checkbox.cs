using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base checkbox control.
    /// </summary>
    public class Checkbox : GuiControl, ICheckbox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkbox"/> class.
        /// </summary>
        public Checkbox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Checkbox"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Checkbox(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA checkbox control type.
        /// </summary>
        public override ControlType UiaType => ControlType.CheckBox;

        /// <summary>
        /// Gets a value indicating whether checkbox is checked.
        /// </summary>
        public virtual bool Checked => 
            Instance.GetPattern<TogglePattern>().Current.ToggleState == ToggleState.On;

        /// <summary>
        /// Sets checkbox checked state
        /// </summary>
        /// <param name="isChecked">true - to check; false - to uncheck</param>
        /// <returns>true - is state was changed; false - if already in specified state</returns>
        public virtual bool SetCheckedState(bool isChecked) =>
            isChecked ? Check() : Uncheck();

        private bool Check()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Check {ToString()}");
            if (Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to check (checked by default)");
                return false;
            }

            Instance.GetPattern<TogglePattern>().Toggle();
            Logger.Instance.Log(LogLevel.Trace, "Checked");

            return true;
        }

        private bool Uncheck()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Uncheck {ToString()}");
            if (!Checked)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to uncheck (unchecked by default)");
                return false;
            }

            Instance.GetPattern<TogglePattern>().Toggle();
            Logger.Instance.Log(LogLevel.Trace, "Unchecked");

            return true;
        }
    }
}
