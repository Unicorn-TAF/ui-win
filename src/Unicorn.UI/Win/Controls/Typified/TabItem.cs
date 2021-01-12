using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base tab item control.
    /// </summary>
    public class TabItem : WinControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        public TabItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public TabItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tab item control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_TabItemControlTypeId;

        /// <summary>
        /// Gets a value indicating whether tab item is selected.
        /// </summary>
        public virtual bool Selected
        {
            get
            {
                var selectionItem = SelectionItemPattern;

                if (selectionItem != null)
                {
                    return selectionItem.CurrentIsSelected != 0;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets selection pattern instance.
        /// </summary>
        protected IUIAutomationSelectionItemPattern SelectionItemPattern => 
            Instance.GetPattern<IUIAutomationSelectionItemPattern>();

        /// <summary>
        /// Selects the tab item.
        /// </summary>
        /// <returns>true - if selection was made; false - if it is already selected</returns>
        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {ToString()}");

            if (Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            var selectionItem = SelectionItemPattern;

            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                Logger.Instance.Log(LogLevel.Trace, "SelectionItemPattern was not found");
                Click();
            }

            Logger.Instance.Log(LogLevel.Trace, "Selected");
            return true;
        }
    }
}