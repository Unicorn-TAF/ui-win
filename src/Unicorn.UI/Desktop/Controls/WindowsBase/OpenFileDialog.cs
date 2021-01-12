using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;
using Unicorn.UI.Desktop.Driver;
using Unicorn.UI.Desktop.PageObject;

namespace Unicorn.UI.Desktop.Controls.WindowsBase
{
    /// <summary>
    /// Describes implementation of windows Open File Dialog.
    /// </summary>
    public class OpenFileDialog : Window
    {
        /// <summary>
        /// Gets or sets 'file name' input control.
        /// </summary>
        [Find(Using.Name, "File name:")]
        public TextInput FileNameInput { get; set; }

        /// <summary>
        /// Gets or sets dialog 'open' button control.
        /// </summary>
        [Find(Using.Id, "1")]
        public Button OpenButton { get; set; }

        /// <summary>
        /// Gets instance of open file dialog.
        /// </summary>
        /// <returns><see cref="OpenFileDialog"/> instance</returns>
        public static OpenFileDialog GetDialog() =>
            GetDialog("Open");

        /// <summary>
        /// Gets instance of open file dialog with specified name.
        /// </summary>
        /// <param name="dialogTitle">open file dialog title</param>
        /// <returns><see cref="OpenFileDialog"/> instance</returns>
        public static OpenFileDialog GetDialog(string dialogTitle) =>
            GuiDriver.Instance.Find<OpenFileDialog>(ByLocator.Name(dialogTitle));

        /// <summary>
        /// Opens specified file
        /// </summary>
        /// <param name="fileName">full file name to open</param>
        public virtual void OpenFile(string fileName)
        {
            FileNameInput.SetValue(fileName);

            if (OpenButton.Exists())
            {
                OpenButton.Click();
            }
            else
            {
                this.Find<SplitButton>(ByLocator.Id("1")).Click();
            }

            WaitForClosed();
        }
    }
}
