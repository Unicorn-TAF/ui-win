using System.Windows.Automation;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base list view control.
    /// </summary>
    public class ListView : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public ListView(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA list view control type.
        /// </summary>
        public override ControlType UiaType => ControlType.List;

        /// <summary>
        /// Selects specified item from list view.
        /// </summary>
        /// <param name="itemName">item name to select</param>
        /// <returns>true - if selection was made; false - if specified item was already selected</returns>
        public virtual bool SelectItem(string itemName) =>
            Find<ListItem>(ByLocator.Name(itemName)).Select();
    }
}
