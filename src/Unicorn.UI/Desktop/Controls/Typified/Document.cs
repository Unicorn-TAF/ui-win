using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base document control.
    /// </summary>
    public class Document : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Document(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA document control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Document;

        /// <summary>
        /// Gets document text.
        /// </summary>
        public override string Text => Instance.GetPattern<TextPattern>()?.DocumentRange.GetText(int.MaxValue);
    }
}
