using System.Windows.Automation;

namespace Unicorn.UI.Desktop.UI.Controls
{
    public class MenuItem : GuiControl
    {
        public override ControlType Type { get { return ControlType.MenuItem; } }



        public MenuItem() { }

        public MenuItem(AutomationElement instance)
            : base(instance)
        {
        }



        public void Select()
        {
            object pattern;

            if (Instance.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
                ((ExpandCollapsePattern)pattern).Expand();
            else if (Instance.TryGetCurrentPattern(TogglePattern.Pattern, out pattern))
                ((TogglePattern)pattern).Toggle();
            else
                ((InvokePattern)Instance.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }


        public bool IsElementToggledOn()
        {
            if (Instance == null)
            {
                // TODO: Invalid parameter error handling.
                return false;
            }

            System.Object objPattern;
            TogglePattern togPattern;
            if (true == Instance.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
            {
                togPattern = objPattern as TogglePattern;
                return togPattern.Current.ToggleState == ToggleState.On;
            }
            // TODO: Object doesn't support TogglePattern error handling.
            return false;
        }
    }
}
