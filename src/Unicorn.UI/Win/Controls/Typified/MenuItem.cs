using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class MenuItem : WinControl
    {
        public MenuItem()
        {
        }

        public MenuItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_MenuItemControlTypeId;

        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => this.GetPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;

        protected IUIAutomationTogglePattern TogglePattern => this.GetPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;

        protected IUIAutomationInvokePattern InvokePattern => this.GetPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;

        public void Select()
        {
            var expandCollapsePattern = this.ExpandCollapsePattern;

            if (expandCollapsePattern != null)
            {
                expandCollapsePattern.Expand();
            }
            else if (this.TogglePattern != null)
            {
                this.TogglePattern.Toggle();
            }
            else
            {
                this.InvokePattern.Invoke();
            }
        }
    }
}
