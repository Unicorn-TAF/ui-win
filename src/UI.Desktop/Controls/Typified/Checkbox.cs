using System.Windows.Automation;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Checkbox : GuiControl, ICheckbox
    {
        public Checkbox()
        {
        }

        public Checkbox(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.CheckBox;

        public bool Checked => GetPattern<TogglePattern>().Current.ToggleState == ToggleState.On;

        public bool Check()
        {
            if (this.Checked)
            {
                return false;
            }

            var pattern = GetPattern<TogglePattern>();
            this.Toggle(pattern);

            return true;
        }

        public bool Uncheck()
        {
            if (!this.Checked)
            {
                return false;
            }

            var pattern = GetPattern<TogglePattern>();
            this.Toggle(pattern);

            return true;
        }

        private void Toggle(TogglePattern pattern)
        {
            pattern.Toggle();
        }
    }
}
