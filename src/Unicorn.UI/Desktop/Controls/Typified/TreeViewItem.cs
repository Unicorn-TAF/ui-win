using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class TreeViewItem : GuiControl, ISelectable
    {
        public TreeViewItem()
        {
        }

        public TreeViewItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.TreeItem;

        public virtual bool Selected => GetPattern<SelectionItemPattern>().Current.IsSelected;

        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Selecting {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (already selected)");
                return false;
            }

            var pattern = GetPattern<SelectionItemPattern>();

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
