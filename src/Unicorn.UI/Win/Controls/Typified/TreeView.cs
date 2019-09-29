using System;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class TreeView : WinContainer
    {
        public TreeView()
        {
        }

        public TreeView(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_TreeControlTypeId;

        public virtual bool SelectItem(string itemName) =>
            GetTreeViewItem(itemName).Select();

        public virtual bool SelectItemInHierarhy(params string[] hierarhy)
        {
            if (hierarhy.Length == 0)
            {
                throw new ArgumentException("Nothing to select, please specify at least one item");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Select hierarhy '{string.Join(" > ", hierarhy)}'");

            WinControl parent = this;

            for (var i = 0; i < hierarhy.Length - 1; i++)
            {
                var treeItem = parent.Find<TreeItem>(ByLocator.Name(hierarhy[i]));
                ExpandParentNode(treeItem);
                parent = treeItem;
            }

            return parent
                .Find<TreeItem>(ByLocator.Name(hierarhy[hierarhy.Length - 1]))
                .Select();
        }

        protected virtual TreeItem GetTreeViewItem(string itemName) =>
            this.Find<TreeItem>(ByLocator.Name(itemName));

        private void ExpandParentNode(TreeItem parentNode)
        {
            var expand = parentNode
                .Instance
                .GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId)
                as IUIAutomationExpandCollapsePattern;

            if (expand != null)
            {
                Logger.Instance.Log(LogLevel.Trace, $"Expanding parent '{parentNode.ToString()}'");
                expand.Expand();
            }
        }
    }
}
