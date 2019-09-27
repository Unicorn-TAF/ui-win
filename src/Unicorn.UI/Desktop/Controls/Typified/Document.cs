using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Document : GuiControl
    {
        public Document()
        {
        }

        public Document(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Document;

        public override string Text => this.GetPattern<TextPattern>()?.DocumentRange.GetText(int.MaxValue);
    }
}
