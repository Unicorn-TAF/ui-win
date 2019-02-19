using System;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class TextInput : GuiControl, ITextInput
    {
        public TextInput()
        {
        }

        public TextInput(AutomationElement instance) : base(instance)
        {
        }

        public override ControlType Type => ControlType.Edit;

        public string Value
        {
            get
            {
                if (Instance.Current.ClassName.Equals("PasswordBox"))
                {
                    return "The field is of PasswordBox type. Unable to get value";
                }
                else
                {
                    return GetPattern<ValuePattern>().Current.Value;
                }
            }
        }

        public void SendKeys(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {this.ToString()}");

            var pattern = GetPattern<ValuePattern>();

            if (pattern.Current.IsReadOnly)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            pattern.SetValue(this.Value + text);
        }

        public bool SetText(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set text '{text}' to {this.ToString()}");

            var pattern = GetPattern<ValuePattern>();

            if (pattern.Current.IsReadOnly)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            if (!this.Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                pattern.SetValue(text);
                return true;
            }
            else
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to set (input already has such text)");
                return false;
            }
        }
    }
}
