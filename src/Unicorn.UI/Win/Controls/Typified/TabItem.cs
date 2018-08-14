using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class TabItem : WinControl, ISelectable
    {
        public TabItem()
        {
        }

        public TabItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_TabItemControlTypeId;

        protected IUIAutomationSelectionItemPattern SelectionItemPattern => base.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

        public bool Selected
        {
            get
            {
                var selectionItem = this.SelectionItemPattern;

                if (selectionItem != null)
                {
                    return selectionItem.CurrentIsSelected != 0;
                }

                return false;
            }
        }

        public bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (selected by default)");
                return false;
            }

            var selectionItem = this.SelectionItemPattern;

            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                Logger.Instance.Log(LogLevel.Trace, "\tSelectionItemPattern was not found");
                base.Click();
            }

            Logger.Instance.Log(LogLevel.Trace, "\tSelected");
            return true;
        }
    }
}