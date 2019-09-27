using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class ProgressBar : GuiControl
    {
        public ProgressBar()
        {
        }

        public ProgressBar(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.ProgressBar;

        public double CurrentProgress => this.GetPattern<RangeValuePattern>().Current.Value;
    }
}
