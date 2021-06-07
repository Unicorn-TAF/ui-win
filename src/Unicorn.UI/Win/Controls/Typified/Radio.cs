using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base radio button control.
    /// </summary>
    public class Radio : WinControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Radio"/> class.
        /// </summary>
        public Radio()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radio"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Radio(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA radio button control type.
        /// </summary>
        public override int UiaType => 
            UIA_ControlTypeIds.UIA_RadioButtonControlTypeId;

        /// <summary>
        /// Gets a value indicating whether radio is selected.
        /// </summary>
        public virtual bool Selected => 
            SelectionItemPattern.CurrentIsSelected != 0;

        /// <summary>
        /// Gets selection pattern instance.
        /// </summary>
        protected IUIAutomationSelectionItemPattern SelectionItemPattern => 
            Instance.GetPattern<IUIAutomationSelectionItemPattern>();

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

            var pattern = SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Trace, "Selected");

            return true;
        }
    }
}
