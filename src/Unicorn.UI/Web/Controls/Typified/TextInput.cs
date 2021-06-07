using System;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    /// <summary>
    /// Describes base text input control.
    /// </summary>
    public class TextInput : WebControl, ITextInput
    {
        /// <summary>
        /// Gets text input value
        /// </summary>
        public virtual string Value => Instance.GetAttribute("value");

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

            Instance.SendKeys(text);
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

            if (!Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                Instance.Clear();
                Instance.SendKeys(text);
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
