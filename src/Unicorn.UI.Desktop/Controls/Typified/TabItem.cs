using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class TabItem : GuiControl, ISelectable
    {
        public TabItem()
        {
        }

        public TabItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.TabItem;

        public bool Selected
        {
            get
            {
                var selectionItem = GetPattern<SelectionItemPattern>();
                if (selectionItem != null)
                {
                    return selectionItem.Current.IsSelected;
                }

                return false;
            }
        }

        public bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Debug, "\tNo need to select (selected by default)");
                return false;
            }

            var selectionItem = GetPattern<SelectionItemPattern>();
            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                
                var invoke = GetPattern<InvokePattern>();
                if (invoke != null)
                {
                    Logger.Instance.Log(LogLevel.Debug, "\tSelectionItemPattern was not found, trying to call Invoke");
                    invoke.Invoke();
                }
                else
                {
                    Logger.Instance.Log(LogLevel.Debug, "\tSelectionItemPattern was not found, trying to click");
                    this.Click();
                }
            }

            Logger.Instance.Log(LogLevel.Debug, "\tSelected");
            return true;
        }
    }
}