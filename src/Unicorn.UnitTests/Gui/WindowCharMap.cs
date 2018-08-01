using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UnitTests.Gui
{
    public class WindowCharMap : Window
    {
        [Find(Using.Name, "Help")]
        public Button ButtonHelp;

        [Find(Using.Name, "Select")]
        public Button ButtonSelect;

        [Find(Using.Name, "Copy")]
        public Button ButtonCopy;

        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts;

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy;

        [Find(Using.Name, "Advanced view")]
        public Checkbox CheckboxAdvancedView;

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet;

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy;

        [Find(Using.Id, "133")]
        public Button ButtonSearch;

        #endregion
    }
}
