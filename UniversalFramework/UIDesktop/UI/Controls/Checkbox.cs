using System.Windows.Automation;
using Unicorn.UICore.UI.Controls;

namespace Unicorn.UIDesktop.UI.Controls
{
    public class Checkbox : GuiControl, ICheckbox
    {
        public Checkbox() { }

        public Checkbox(AutomationElement instance)
			: base(instance)
		{
        }

        public bool Checked
        {
            get
            {
                return GetPattern<TogglePattern>().Current.ToggleState == ToggleState.On;
            }
        }

        public override ControlType Type { get { return ControlType.CheckBox; } }

        public bool Check()
        {
            if (Checked)
                return false;

            var pattern = GetPattern<TogglePattern>();
                Toggle(pattern);

            return true;
        }

        public bool Uncheck()
        {
            if (!Checked)
                return false;

            var pattern = GetPattern<TogglePattern>();
                Toggle(pattern);

            return true;
        }

        private void Toggle(TogglePattern pattern)
        {
            pattern.Toggle();
        }
    }
}
