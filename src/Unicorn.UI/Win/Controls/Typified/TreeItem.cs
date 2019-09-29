using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base tree item control.
    /// </summary>
    public class TreeItem : WinControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItem"/> class.
        /// </summary>
        public TreeItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItem"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public TreeItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tree item control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ListItemControlTypeId;

        /// <summary>
        /// Gets a value indicating whether tree item is selected.
        /// </summary>
        public virtual bool Selected => 
            this.SelectionItemPattern.CurrentIsSelected != 0;

        /// <summary>
        /// Gets selection pattern instance.
        /// </summary>
        protected IUIAutomationSelectionItemPattern SelectionItemPattern => 
            this.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

        /// <summary>
        /// Selects the tree item.
        /// </summary>
        /// <returns>true - if selection was made; false - if the item is already selected</returns>
        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Selecting {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (already selected)");
                return false;
            }

            var pattern = this.SelectionItemPattern;

            if (pattern != null)
            {
                pattern.Select();
            }
            else
            {
                this.Click();
            }

            Logger.Instance.Log(LogLevel.Trace, "Selected");
            return true;
        }
    }
}
