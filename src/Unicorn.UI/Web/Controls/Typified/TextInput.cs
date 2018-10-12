using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class TextInput : WebControl, ITextInput
    {
        public virtual string Value => this.Instance.GetAttribute("value");

        public virtual void SendKeys(string text)
        {
            this.Instance.SendKeys(text);
        }

        public virtual bool SetText(string text)
        {
            this.Instance.Clear();
            this.Instance.SendKeys(text);
            return true;
        }
    }
}
