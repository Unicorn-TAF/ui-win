using System.Threading;
using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Dropdown : WinControl, IDropdown
    {
        public Dropdown()
        {
        }

        public Dropdown(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_ComboBoxControlTypeId;

        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => base.GetPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;

        protected IUIAutomationSelectionPattern SelectionPattern => base.GetPattern(UIA_PatternIds.UIA_SelectionPatternId) as IUIAutomationSelectionPattern;

        protected IUIAutomationValuePattern ValuePattern => base.GetPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;

        public bool Expanded => this.ExpandCollapsePattern.CurrentExpandCollapseState.Equals(ExpandCollapseState.ExpandCollapseState_Expanded);

        public string SelectedValue
        {
            get
            {
                var selection = this.SelectionPattern;
                if (selection != null)
                {
                    var items = selection.GetCurrentSelection();

                    if (items.Length > 1)
                    {
                        return items.GetElement(0).GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;
                    }
                }

                var value = this.ValuePattern;

                if (value != null)
                {
                    return value.CurrentValue;
                }
                    
                return string.Empty;
            }
        }

        public bool Select(string item)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{item}' item from {this.ToString()}");

            if (item.Equals(this.SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (the item is selected by default)");
                return false;
            }

            var value = this.ValuePattern;

            if (value != null)
            {
                value.SetValue(item);
            }
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(ByLocator.Name(item));

                if (itemEl != null)
                {
                    Logger.Instance.Log(LogLevel.Trace, "\tItem was found. Selecting...");
                    itemEl.Select();
                }
                    
                Collapse();
                Thread.Sleep(500);
            }

            Logger.Instance.Log(LogLevel.Trace, "\tItem was selected");

            return true;
        }

        public bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tExpanding dropdown");
            if (this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to expand (expanded by default)");
                return false;
            }

            this.ExpandCollapsePattern.Expand();
            Logger.Instance.Log(LogLevel.Trace, "\t\tExpanded");
            return true;
        }

        public bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tCollapsing dropdown");
            if (!this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to collapse (collapsed by default)");
                return false;
            }

            this.ExpandCollapsePattern.Collapse();
            Logger.Instance.Log(LogLevel.Trace, "\t\tCollapsed");
            return true;
        }
    }
}
