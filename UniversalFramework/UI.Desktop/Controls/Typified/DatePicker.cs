using System.Windows.Automation;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class DatePicker : GuiControl, IControl, IExpandable
    {
        public DatePicker()
        {
        }

        public DatePicker(AutomationElement instance) : base(instance)
        {
        }

        public override ControlType Type => ControlType.Custom;

        public override string ClassName => "DatePicker";

        public bool Expand()
        {
            if (!Expanded)
            {
                GetPattern<ExpandCollapsePattern>().Expand();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Collapse()
        {
            if (Expanded)
            {
                GetPattern<ExpandCollapsePattern>().Collapse();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Value
        {
            get
            {
                var value = GetPattern<ValuePattern>();

                if (value != null)
                {
                    return value.Current.Value;
                }
                    
                return null;
            }
            set
            {
                var valuePattern = GetPattern<ValuePattern>();

                if (valuePattern != null)
                {
                    valuePattern.SetValue(value);
                }
                    
            }
        }

        public bool Expanded
        {
            get
            {
                return GetPattern<ExpandCollapsePattern>().Current.ExpandCollapseState == ExpandCollapseState.Expanded;
            }
        }
    }
}
