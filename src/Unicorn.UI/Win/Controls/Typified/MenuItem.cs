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

        public override int Type => UIA_ControlTypeIds.UIA_MenuItemControlTypeId;

        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => base.GetPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;

        protected IUIAutomationTogglePattern TogglePattern => base.GetPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;

        protected IUIAutomationInvokePattern InvokePattern => base.GetPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;

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

        public bool IsElementToggledOn()
        {
            if (this.Instance == null)
            {
                // TODO: Invalid parameter error handling.
                return false;
            }

            if (this.TogglePattern != null)
            {
                return TogglePattern.CurrentToggleState.Equals(ToggleState.ToggleState_On);
            }

            // TODO: Object doesn't support TogglePattern error handling.
            return false;
        }
    }
}
