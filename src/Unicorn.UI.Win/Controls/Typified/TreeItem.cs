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
        public override int UiaType => UIA_ControlTypeIds.UIA_TreeItemControlTypeId;

        /// <summary>
        /// Gets a value indicating whether tree item is selected.
        /// </summary>
        public virtual bool Selected => 
            SelectionItemPattern.CurrentIsSelected != 0;

        /// <summary>
        /// Gets selection pattern instance.
        /// </summary>
        protected IUIAutomationSelectionItemPattern SelectionItemPattern => 
            Instance.GetPattern<IUIAutomationSelectionItemPattern>();

        /// <summary>
        /// Selects the tree item.
        /// </summary>
        /// <returns>true - if selection was made; false - if the item is already selected</returns>
        public virtual bool Select()
        {
            ULog.Debug("Selecting {0}", this);

            if (Selected)
            {
                ULog.Trace("No need to select (already selected)");
                return false;
            }

            var pattern = SelectionItemPattern;

            if (pattern != null)
            {
                pattern.Select();
            }
            else
            {
                Click();
            }

            ULog.Trace("Selected");
            return true;
        }

        /// <summary>
        /// Scrolls view to tree item position.
        /// </summary>
        public virtual void ScrollToItem()
        {
            var pattern = Instance.GetPattern<IUIAutomationScrollItemPattern>();

            if (pattern != null)
            {
                pattern.ScrollIntoView();
            }
        }
    }
}
