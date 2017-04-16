using System;
using System.Windows.Automation;
using UICore.UI.Controls;

namespace UIDesktop.UI.Controls
{
    public class TextInput : GuiControl, ITextInput
    {
        public override ControlType Type { get { return ControlType.Edit; } }

        public string Value
        {
            get
            {
                return GetPattern<ValuePattern>().Current.Value;
            }
        }

        public TextInput() { }

        public TextInput(AutomationElement instance)
			: base(instance)
		{
        }


        public void SendKeys(string text)
        {
            var pattern = GetPattern<ValuePattern>();
                if (pattern.Current.IsReadOnly)
                    throw new Exception("Input is disabled");

            GetPattern<ValuePattern>().SetValue(text);
        }
    }
}
