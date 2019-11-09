using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base radio button control.
    /// </summary>
    public class Radio : GuiControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Radio"/> class.
        /// </summary>
        public Radio()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radio"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Radio(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA radio button control type.
        /// </summary>
        public override ControlType UiaType => ControlType.RadioButton;

        /// <summary>
        /// Gets a value indicating whether radio is selected.
        /// </summary>
        public virtual bool Selected =>
            (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern)
            .Current
            .IsSelected;

        /// <summary>
        /// Selects the radio button.
        /// </summary>
        /// <returns>true - if selection was made; false - if radio is already selected</returns>
        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {ToString()}");

            if (Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            var pattern = Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Trace, "Selected");

            return true;
        }
    }
}
