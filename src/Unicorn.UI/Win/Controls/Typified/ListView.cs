using System;
using UIAutomationClient;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base list view control.
    /// </summary>
    public class ListView : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public ListView(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA list view control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ListControlTypeId;

        /// <summary>
        /// Selects specified item from list view.
        /// </summary>
        /// <param name="itemName">item name to select</param>
        /// <returns>true - if selection was made; false - if specified item was already selected</returns>
        public virtual bool SelectItem(string itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }

            return Find<ListItem>(ByLocator.Name(itemName)).Select();
        }
    }
}
