using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base list item control.
    /// </summary>
    public class ListItem : GuiControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class.
        /// </summary>
        public ListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItem"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public ListItem(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA list item control type.
        /// </summary>
        public override ControlType UiaType => ControlType.ListItem;

        /// <summary>
        /// Gets a value indicating whether item is selected.
        /// </summary>
        public virtual bool Selected =>
            Instance.GetPattern<SelectionItemPattern>().Current.IsSelected;

        /// <summary>
        /// Selects the list item.
        /// </summary>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Selecting {ToString()}");

            if (Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (already selected)");
                return false;
            }

            var pattern = Instance.GetPattern<SelectionItemPattern>();
            if (pattern != null)
            {
                pattern.Select();
            }
            else
            {
                Click();
            }

            Logger.Instance.Log(LogLevel.Trace, "Selected");
            return true;
        }

        /// <summary>
        /// Scrolls view to list item position.
        /// </summary>
        public virtual void ScrollToItem()
        {
            var pattern = Instance.GetPattern<ScrollItemPattern>();

            if (pattern != null)
            {
                pattern.ScrollIntoView();
            }
        }
    }
}
