using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base tab item control.
    /// </summary>
    public class TabItem : GuiControl, ISelectable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        public TabItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public TabItem(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tab item control type.
        /// </summary>
        public override ControlType UiaType => ControlType.TabItem;

        /// <summary>
        /// Gets a value indicating whether tab item is selected.
        /// </summary>
        public virtual bool Selected
        {
            get
            {
                var selectionItem = Instance.GetPattern<SelectionItemPattern>();
                if (selectionItem != null)
                {
                    return selectionItem.Current.IsSelected;
                }

                return false;
            }
        }

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

            var selectionItem = Instance.GetPattern<SelectionItemPattern>();
            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                var invoke = Instance.GetPattern<InvokePattern>();
                if (invoke != null)
                {
                    Logger.Instance.Log(LogLevel.Trace, "SelectionItemPattern was not found, trying to call Invoke");
                    invoke.Invoke();
                }
                else
                {
                    Logger.Instance.Log(LogLevel.Trace, "SelectionItemPattern was not found, trying to click");
                    Click();
                }
            }

            Logger.Instance.Log(LogLevel.Trace, "Selected");
            return true;
        }
    }
}