using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Radio : WinControl, ISelectable
    {
        public Radio()
        {
        }

        public Radio(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_RadioButtonControlTypeId;

        public bool Selected => this.SelectionItemPattern.CurrentIsSelected != 0;

        protected IUIAutomationSelectionItemPattern SelectionItemPattern => this.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

        public bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            var pattern = this.SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Trace, "Selected");

            return true;
        }
    }
}
