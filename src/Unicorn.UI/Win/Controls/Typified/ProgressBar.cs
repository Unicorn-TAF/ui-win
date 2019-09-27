using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class ProgressBar : WinControl
    {
        public ProgressBar()
        {
        }

        public ProgressBar(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ProgressBarControlTypeId;

        public double CurrentProgress => ValuePattern.CurrentValue;


        protected IUIAutomationRangeValuePattern ValuePattern =>
            this.GetPattern(UIA_PatternIds.UIA_RangeValuePatternId) as IUIAutomationRangeValuePattern;
    }
}
