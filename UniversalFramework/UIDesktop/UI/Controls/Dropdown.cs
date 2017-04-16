using System;
using System.Windows.Automation;
using UICore.UI.Controls;

namespace UIDesktop.UI.Controls
{
    class Dropdown : GuiControl, IDropdown
    {
        public Dropdown() { }

        public Dropdown(AutomationElement instance)
            : base(instance)
        {
        }

        public bool isExpanded
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override ControlType Type { get { return ControlType.ComboBox; } }

        public void CheckItems(string[] items)
        {
            throw new NotImplementedException();
        }

        public bool Select(string item)
        {
            throw new NotImplementedException();
        }
    }
}
