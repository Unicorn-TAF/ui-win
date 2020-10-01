using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base menu item control.
    /// </summary>
    public class MenuItem : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        public MenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public MenuItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA menu item control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_MenuItemControlTypeId;

        /// <summary>
        /// Gets expand/collapse pattern instance.
        /// </summary>
        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => 
            Instance.GetPattern<IUIAutomationExpandCollapsePattern>();

        /// <summary>
        /// Gets toggle pattern instance.
        /// </summary>
        protected IUIAutomationTogglePattern TogglePattern => 
            Instance.GetPattern<IUIAutomationTogglePattern>();

        /// <summary>
        /// Gets invoke pattern instance.
        /// </summary>
        protected IUIAutomationInvokePattern InvokePattern => 
            Instance.GetPattern<IUIAutomationInvokePattern>();

        /// <summary>
        /// Selects menu item, if menu item is parent item it is expanded.
        /// </summary>
        public virtual void Select()
        {
            var expandCollapsePattern = ExpandCollapsePattern;

            if (expandCollapsePattern != null)
            {
                expandCollapsePattern.Expand();
            }
            else if (TogglePattern != null)
            {
                TogglePattern.Toggle();
            }
            else
            {
                InvokePattern.Invoke();
            }
        }
    }
}
