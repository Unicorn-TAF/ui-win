using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.Charmap.Gui
{
    public class CharmapWindow : Window
    {
        [Name("'Help' button")]
        [ByName("Help")]
        public Button ButtonHelp { get; set; }

        [Name("'Select' button")]
        [ByName("Select")]
        public Button ButtonSelect { get; set; }

        [Name("'Copy' button")]
        [ByName("Copy")]
        public Button ButtonCopy { get; set; }

        [Name("'Font' dropdown")]
        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts { get; set; }

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy { get; set; }

        public Checkbox CheckboxAdvancedView => Find<Checkbox>(ByLocator.Name("Advanced view"));

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet { get; set; }

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy { get; set; }

        [ById("133")]
        public Button ButtonSearch { get; set; }

        #endregion
    }
}
