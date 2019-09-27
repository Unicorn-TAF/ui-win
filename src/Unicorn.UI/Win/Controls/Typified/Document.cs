using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Document : WinControl
    {
        public Document()
        {
        }

        public Document(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_DocumentControlTypeId;

        public override string Text => TextPattern.DocumentRange.GetText(int.MaxValue);

        protected IUIAutomationTextPattern TextPattern =>
            this.GetPattern(UIA_PatternIds.UIA_TextPatternId) as IUIAutomationTextPattern;
    }
}
