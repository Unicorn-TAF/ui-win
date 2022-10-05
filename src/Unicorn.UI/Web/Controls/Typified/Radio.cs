using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Web.Controls.Typified
{
    /// <summary>
    /// Describes base radio button control.
    /// </summary>
    public class Radio : WebControl, ISelectable
    {
        /// <summary>
        /// Gets a value indicating whether radio is selected.
        /// </summary>
        public virtual bool Selected => Instance.Selected;

        /// <summary>
        /// Selects the radio button.
        /// </summary>
        /// <returns>true - if selection was made; false - if radio is already selected</returns>
        public virtual bool Select()
        {
            ULog.Debug("Select {0}", this);

            if (Selected)
            {
                ULog.Trace("No need to select (selected by default)");
                return false;
            }

            Instance.Click();

            ULog.Trace("Selected");

            return true;
        }
    }
}
