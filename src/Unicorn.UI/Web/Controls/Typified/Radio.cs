using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class Radio : WebControl, ISelectable
    {
        public virtual bool Selected => this.Instance.Selected;

        public virtual bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (selected by default)");
                return false;
            }

            this.Instance.Click();

            Logger.Instance.Log(LogLevel.Trace, "Selected");

            return true;
        }
    }
}
