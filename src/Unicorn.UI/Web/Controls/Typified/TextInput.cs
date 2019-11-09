using System;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class TextInput : WebControl, ITextInput
    {
        public virtual string Value => Instance.GetAttribute("value");

        public virtual void SendKeys(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {ToString()}");
            Instance.SendKeys(text);
        }

        public virtual bool SetValue(string text)
        {
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
