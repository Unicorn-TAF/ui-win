using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.Controls.Typified;

namespace Unicorn.UnitTests.Gui.Win
{
    public class WindowCharMap : Window
    {
        private CopyButtonWithDefaultLocator buttonCopyDefaultLocator;

        [Find(Using.Name, "Help")]
        public Button ButtonHelp { get; set; }

        [Find(Using.Name, "Select")]
        public Button ButtonSelect { get; set; }

        [Find(Using.Name, "Copy")]
        public Button ButtonCopy { get; set; }

        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts { get; set; }

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy { get; set; }

        [Find(Using.Name, "Advanced view")]
        public Checkbox CheckboxAdvancedView { get; set; }

        public CopyButtonWithDefaultLocator ButtonCopyDefaultLocator { get; set; }

        public CopyButtonWithDefaultLocator ButtonCopyDefaultLocatorGetter => buttonCopyDefaultLocator;

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet { get; set; }

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy { get; set; }

        [Find(Using.Id, "133")]
        public Button ButtonSearch { get; set; }

        #endregion
    }
}
