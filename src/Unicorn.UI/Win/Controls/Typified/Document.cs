using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base document control.
    /// </summary>
    public class Document : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Document(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA document control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_DocumentControlTypeId;

        /// <summary>
        /// Gets document text.
        /// </summary>
        public override string Text => TextPattern.DocumentRange.GetText(int.MaxValue);

        /// <summary>
        /// Gets text pattern instance.
        /// </summary>
        protected IUIAutomationTextPattern TextPattern =>
            Instance.GetPattern<IUIAutomationTextPattern>();
    }
}
