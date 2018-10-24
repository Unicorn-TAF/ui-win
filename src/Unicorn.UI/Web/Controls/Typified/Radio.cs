using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class Radio : WebControl, ISelectable
    {
        public bool Selected => this.Instance.Selected;

        public bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (selected by default)");
                return false;
            }

            this.Instance.Click();

            Logger.Instance.Log(LogLevel.Trace, "\tSelected");

            return true;
        }
    }
}
