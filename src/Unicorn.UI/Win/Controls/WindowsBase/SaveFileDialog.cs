using System;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.Controls.WindowsBase
{
    /// <summary>
    /// Describes implementation of windows Save File Dialog.
    /// </summary>
    public class SaveFileDialog : Window
    {
        /// <summary>
        /// Gets or sets 'file name' input control.
        /// </summary>
        [Find(Using.Name, "File name:")]
        public TextInput FileNameInput { get; set; }

        /// <summary>
        /// Gets or sets 'file type' dropdown control.
        /// </summary>
        [Find(Using.Id, "FileTypeControlHost")]
        public Dropdown FileTypeDropdown { get; set; }

        /// <summary>
        /// Gets or sets dialog 'open' button control.
        /// </summary>
        [Find(Using.Id, "1")]
        public Button SaveButton { get; set; }

        /// <summary>
        /// Gets instance of save file dialog.
        /// </summary>
        /// <returns><see cref="SaveFileDialog"/> instance</returns>
        public static SaveFileDialog GetDialog() =>
            WinDriver.Instance.Find<SaveFileDialog>(ByLocator.Name("Save As"));

        /// <summary>
        /// Saves specified file
        /// </summary>
        /// <param name="fileName">full file name to save as</param>
        public virtual void SaveFile(string fileName)
        {
            FileNameInput.SetValue(fileName);

            SaveButton.Click();

            WaitForDialogClosing();
        }

        /// <summary>
        /// Saves specified file with specified file type
        /// </summary>
        /// <param name="fileName">full file name to save as</param>
        /// <param name="fileType">file type</param>
        public virtual void SaveFile(string fileName, string fileType)
        {
            FileTypeDropdown.Select(fileType);
            FileNameInput.SetValue(fileName);

            SaveButton.Click();

            WaitForDialogClosing();
        }

        private void WaitForDialogClosing() =>
            new DefaultWait
            {
                Timeout = TimeSpan.FromSeconds(5),
                PollingInterval = TimeSpan.FromMilliseconds(250),
                ErrorMessage = "Failed to wait for window is closed!"
            }
            .Until(() => !WinDriver.Instance.TryGetChild<SaveFileDialog>(Locator, 0));
    }
}
