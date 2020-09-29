using System;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base tree view control.
    /// </summary>
    public class TreeView : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public TreeView(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tree view control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Tree;

        /// <summary>
        /// Selects specified tree item.
        /// </summary>
        /// <param name="itemName">item name to select</param>
        /// <returns>true - if selection was made; false - if the item is already selected</returns>
        public virtual bool SelectItem(string itemName) =>
            GetTreeViewItem(itemName).Select();

        /// <summary>
        /// Selects specified tree item specified by hierarchy.
        /// </summary>
        /// <param name="hierarchy">list of items by hierarchy</param>
        /// <returns>true - if selection was made; false - if the item is already selected</returns>
        public virtual bool SelectItemInHierarchy(params string[] hierarchy)
        {
            if (hierarchy.Length == 0)
            {
                throw new ArgumentException("Nothing to select, please specify at least one item");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Select hierarhy '{string.Join(" > ", hierarchy)}'");

            GuiControl parent = this;

            for (var i = 0; i < hierarchy.Length - 1; i++)
            {
                var treeItem = parent.Find<TreeItem>(ByLocator.Name(hierarchy[i]));
                ExpandParentNode(treeItem);
                parent = treeItem;
            }

            return parent
                .Find<TreeItem>(ByLocator.Name(hierarchy[hierarchy.Length - 1]))
                .Select();
        }

        /// <summary>
        /// Gets child tree item by name.
        /// </summary>
        /// <param name="itemName">tree item name</param>
        /// <returns>tree item instance</returns>
        protected virtual TreeItem GetTreeViewItem(string itemName) =>
            Find<TreeItem>(ByLocator.Name(itemName));

        private void ExpandParentNode(TreeItem parentNode)
        {
            object patternObject;

            parentNode
                .Instance
                .TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out patternObject);

            var expand = patternObject as ExpandCollapsePattern;

            if (expand != null)
            {
                Logger.Instance.Log(LogLevel.Trace, $"Expanding parent '{parentNode.ToString()}'");
                expand.Expand();
            }
        }
    }
}
