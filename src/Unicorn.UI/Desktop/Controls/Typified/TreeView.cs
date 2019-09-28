using System;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class TreeView : GuiContainer
    {
        public TreeView()
        {
        }

        public TreeView(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Tree;

        public virtual bool SelectItem(string itemName) =>
            GetTreeViewItem(itemName).Select();

        public virtual bool SelectItemInHierarhy(params string[] hierarhy)
        {
            if (hierarhy.Length == 0)
            {
                throw new ArgumentException("Nothing to select, please specify at least one item");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Select hierarhy '{string.Join(" > ", hierarhy)}'");

            GuiControl parent = this;

            for (var i = 0; i <  hierarhy.Length - 1; i++)
            {
                var treeItem = parent.Find<TreeViewItem>(ByLocator.Name(hierarhy[i]));
                ExpandParentNode(treeItem);
                parent = treeItem;
            }

            return parent
                .Find<TreeViewItem>(ByLocator.Name(hierarhy[hierarhy.Length - 1]))
                .Select();
        }

        protected virtual TreeViewItem GetTreeViewItem(string itemName) =>
            this.Find<TreeViewItem>(ByLocator.Name(itemName));

        private void ExpandParentNode(TreeViewItem parentNode)
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
