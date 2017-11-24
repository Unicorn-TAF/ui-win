﻿using System;
using System.Windows.Automation;
using Unicorn.UI.Core.UI;
using Unicorn.UI.Core.UI.Controls;

namespace Unicorn.UI.Desktop.UI.Controls
{
    public class TextInput : GuiControl, IControl, ITextInput
    {
        public override ControlType Type { get { return ControlType.Edit; } }

        public string Value
        {
            get
            {
                if (Instance.Current.ClassName.Equals("PasswordBox"))
                    return "The field is of PasswordBox type. Unable to get value";
                else
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
            this.WaitForEnabled();

            var pattern = GetPattern<ValuePattern>();
                if (pattern.Current.IsReadOnly)
                    throw new Exception("Input is disabled");

            if(!Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
                GetPattern<ValuePattern>().SetValue(text);
        }
    }
}