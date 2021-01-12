using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base menu item control.
    /// </summary>
    public class MenuItem : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        public MenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public MenuItem(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA menu item control type.
        /// </summary>
        public override ControlType UiaType => ControlType.MenuItem;

        /// <summary>
        /// Selects menu item, if menu item is parent item it is expanded.
        /// </summary>
        public virtual void Select()
        {
            object pattern;

            if (Instance.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
            {
                ((ExpandCollapsePattern)pattern).Expand();
            }
            else if (Instance.TryGetCurrentPattern(TogglePattern.Pattern, out pattern))
            {
                ((TogglePattern)pattern).Toggle();
            }
            else
            {
                ((InvokePattern)Instance.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
            }
        }
    }
}
