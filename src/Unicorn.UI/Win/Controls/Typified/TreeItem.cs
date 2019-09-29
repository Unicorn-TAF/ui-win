using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class TreeItem : WinControl, ISelectable
    {
        public TreeItem()
        {
        }

        public TreeItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ListItemControlTypeId;

        public virtual bool Selected => this.SelectionItemPattern.CurrentIsSelected != 0;

        protected IUIAutomationSelectionItemPattern SelectionItemPattern => this.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

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
