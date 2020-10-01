using System;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base text input control.
    /// </summary>
    public class TextInput : GuiControl, ITextInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInput"/> class.
        /// </summary>
        public TextInput()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInput"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public TextInput(AutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA text input control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Edit;

        /// <summary>
        /// Gets a value indicating whether input is password input.
        /// </summary>
        public bool IsPasswordType =>
            Instance.Current.ClassName.Equals("PasswordBox");

        /// <summary>
        /// Gets text input value
        /// </summary>
        public virtual string Value =>
            IsPasswordType ?
            "The field is of PasswordBox type. Unable to get value" :
            Instance.GetPattern<ValuePattern>().Current.Value;

        /// <summary>
        /// Adds text to already existing input value.
        /// </summary>
        /// <param name="text">text to send</param>
        public virtual void SendKeys(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {ToString()}");

            var pattern = Instance.GetPattern<ValuePattern>();

            if (pattern.Current.IsReadOnly)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            pattern.SetValue(Value + text);
        }

        /// <summary>
        /// Set text input value (overwriting existing one)
        /// </summary>
        /// <param name="text">text to send</param>
        /// <returns>true - if value was set; false - if input already has specified value</returns>
        public virtual bool SetValue(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set text '{text}' to {ToString()}");

            var pattern = Instance.GetPattern<ValuePattern>();

            if (pattern.Current.IsReadOnly)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            if (!Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
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
