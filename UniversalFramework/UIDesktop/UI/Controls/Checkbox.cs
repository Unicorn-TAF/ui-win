using System.Windows.Automation;
using UICore.UI.Controls;

namespace UIDesktop.UI.Controls
{
    class Checkbox : GuiControl, ICheckbox
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
            var pattern = GetPattern<TogglePattern>();
            if (pattern.Current.ToggleState == ToggleState.Off)
                Toggle(pattern);

            return true;
        }

        public void Uncheck()
        {
            var pattern = GetPattern<TogglePattern>();
            if (pattern.Current.ToggleState == ToggleState.On)
                Toggle(pattern);
        }

        private void Toggle(TogglePattern pattern)
        {
            pattern.Toggle();
        }
    }
}
