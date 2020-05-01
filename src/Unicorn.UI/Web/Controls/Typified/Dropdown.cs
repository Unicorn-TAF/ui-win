using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    public class Dropdown : WebControl, IDropdown
    {
        private SelectElement selectInstance = null;

        public bool Expanded => throw new NotImplementedException();

        public string SelectedValue => SelectControl.SelectedOption.Text;

        private SelectElement SelectControl
        {
            get
            {
                if (selectInstance == null)
                {
                    selectInstance = new SelectElement(Instance);
                }

                return selectInstance;
            }
        }

        public bool Collapse()
        {
            throw new NotImplementedException();
        }

        public bool Expand()
        {
            throw new NotImplementedException();
        }

        public bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {ToString()}");

            if (SelectedValue.Equals(itemName))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            SelectControl.SelectByText(itemName);
            Logger.Instance.Log(LogLevel.Trace, "Item was selected");

            return true;
        }

        public string[] GetOptions() => 
            SelectControl.Options.Select(o => o.Text).ToArray();
    }
}
