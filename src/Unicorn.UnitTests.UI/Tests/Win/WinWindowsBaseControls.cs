using NUnit.Framework;
using System.Reflection;
using System.Threading;
using Unicorn.UI.Win.Controls.WindowsBase;

namespace Unicorn.UnitTests.UI.Tests.Win
{
    [TestFixture]
    public class WinWindowsBaseControls : WinTestsBase
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Open file dialog 'Open File' functionality")]
        public void TestOpenFileDialogOpenFile()
        {
            var fileName = Assembly.GetExecutingAssembly().Location;
            var dialog = CallSystemOpenFileDialog();
            OpenFileDialog.GetDialog().OpenFile(fileName);
            Assert.AreEqual(fileName, dialog.FileName);
            dialog.Dispose();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Open file dialog controls")]
        public void TestOpenFileDialogControls()
        {
            var systemDialog = CallSystemOpenFileDialog();
            var dialog = OpenFileDialog.GetDialog();
            Assert.AreEqual(dialog.FileNameInput.Value, string.Empty);
            Assert.IsTrue(dialog.OpenButton.Enabled);
            dialog.Close();
            systemDialog.Dispose();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Save file dialog 'Save File' functionality")]
        public void TestSaveFileDialogSaveFile()
        {
            var fileName = Assembly.GetExecutingAssembly().Location + "qwe";
            var dialog = CallSystemSaveFileDialog();
            SaveFileDialog.GetDialog().SaveFile(fileName);
            Assert.AreEqual(fileName, dialog.FileName);
            dialog.Dispose();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Save file dialog controls")]
        public void TestSaveFileDialogControls()
        {
            var systemDialog = CallSystemSaveFileDialog();
            var dialog = SaveFileDialog.GetDialog();
            Assert.AreEqual(dialog.FileNameInput.Value, string.Empty);
            Assert.IsTrue(dialog.SaveButton.Enabled);
            dialog.Close();
            systemDialog.Dispose();
        }

        private System.Windows.Forms.OpenFileDialog CallSystemOpenFileDialog()
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            var dialogThread = new Thread(() => dialog.ShowDialog());
            dialogThread.SetApartmentState(ApartmentState.STA);
            dialogThread.Start();
            return dialog;
        }

        private System.Windows.Forms.SaveFileDialog CallSystemSaveFileDialog()
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();
            var dialogThread = new Thread(() => dialog.ShowDialog());
            dialogThread.SetApartmentState(ApartmentState.STA);
            dialogThread.Start();
            return dialog;
        }
    }
}
