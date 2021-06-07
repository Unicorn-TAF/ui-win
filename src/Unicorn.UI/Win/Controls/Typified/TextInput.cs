using System;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base text input control.
    /// </summary>
    public class TextInput : WinControl, ITextInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInput"/> class.
        /// </summary>
        public TextInput()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInput"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public TextInput(IUIAutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA text input control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_EditControlTypeId;

        /// <summary>
        /// Gets a value indicating whether input is password input.
        /// </summary>
        public bool IsPasswordType =>
            Instance.CurrentClassName.Equals("PasswordBox");

        /// <summary>
        /// Gets text input value
        /// </summary>
        public virtual string Value =>
            IsPasswordType ?
            "The field is of PasswordBox type. Unable to get value" :
            ValuePattern.CurrentValue;

        /// <summary>
        /// Gets value pattern instance.
        /// </summary>
        protected IUIAutomationValuePattern ValuePattern => 
            Instance.GetPattern<IUIAutomationValuePattern>();

        /// <summary>
        /// Adds text to already existing input value.
        /// </summary>
        /// <param name="text">text to send</param>
        public virtual void SendKeys(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {ToString()}");

            var pattern = ValuePattern;

            if (pattern.CurrentIsReadOnly != 0)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            pattern.SetValue(text);
        }

        /// <summary>
        /// Set text input value (overwriting existing one)
        /// </summary>
        /// <param name="text">text to send</param>
        /// <returns>true - if value was set; false - if input already has specified value</returns>
        public virtual bool SetValue(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Logger.Instance.Log(LogLevel.Debug, $"Set text '{text}' to {ToString()}");

            var pattern = ValuePattern;

            if (pattern.CurrentIsReadOnly != 0)
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
