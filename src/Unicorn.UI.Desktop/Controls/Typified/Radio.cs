﻿using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Radio : GuiControl, ISelectable
    {
        public Radio()
        {
        }

        public Radio(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.RadioButton;

        public bool Selected
        {
            get
            {
                return (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected;
            }
        }

        public bool Select()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select {this.ToString()}");

            if (this.Selected)
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (selected by default)");
                return false;
            }

            var pattern = this.Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

            pattern.Select();
            Logger.Instance.Log(LogLevel.Trace, "\tSelected");

            return true;
        }
    }
}
