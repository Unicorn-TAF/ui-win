using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
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

        public override int UiaType => UIA_ControlTypeIds.UIA_TabItemControlTypeId;

        public virtual bool Selected
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

        protected IUIAutomationSelectionItemPattern SelectionItemPattern => 
            this.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            var selectionItem = this.SelectionItemPattern;

            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                Logger.Instance.Log(LogLevel.Trace, "SelectionItemPattern was not found");
                this.Click();
            }

            Logger.Instance.Log(LogLevel.Trace, "Selected");
            return true;
        }
    }
}