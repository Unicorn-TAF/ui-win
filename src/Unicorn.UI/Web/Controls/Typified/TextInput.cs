using System;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class TextInput : WebControl, ITextInput
    {
        public virtual string Value => this.Instance.GetAttribute("value");

        public virtual void SendKeys(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {this.ToString()}");

            this.Instance.SendKeys(text);
        }

        public virtual bool SetText(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set text '{text}' to {this.ToString()}");

            if (!this.Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Instance.Clear();
                this.Instance.SendKeys(text);
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
