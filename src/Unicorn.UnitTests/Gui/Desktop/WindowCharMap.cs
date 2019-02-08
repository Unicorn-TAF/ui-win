using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;
using Unicorn.UnitTests.Gui.Desktop;

namespace Unicorn.UnitTests.Gui
{
    public class WindowCharMap : Window
    {
        [Find(Using.Name, "Copy")]
        public Button ButtonCopy { get; set; }

        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts { get; set; }

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy { get; set; }

        [Find(Using.Name, "Advanced view")]
        public Checkbox CheckboxAdvancedView { get; set; }

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet { get; set; }

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy { get; set; }

        [Find(Using.Id, "133")]
        public Button ButtonSearch { get; set; }

        #endregion

        public Button SelectButton => this.ButtonSelect;

        public Button ButtonHelp => this.Find<Button>(ByLocator.Name("Help"));

        [Find(Using.Name, "Select")]
        protected Button ButtonSelect { get; set; }

        [Find(Using.Name, "Font :")]
        [Define((int)DropdownElement.ExpandCollapse, Using.Id, "DropDown")]
        [Define((int)DropdownElement.List, Using.Class, "ComboLBox")]
        [Define((int)DropdownElement.ListItem, Using.Class, "")]
        public GuiDinamicDropdown Droppik;
    }
}
