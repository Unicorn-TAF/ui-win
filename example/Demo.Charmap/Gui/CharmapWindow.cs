using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.Charmap.Gui
{
    public class CharmapWindow : Window
    {
        /// <summary>
        /// Ability to use private fields as page object elements.
        /// </summary>
        [Name("'Copy' button")]
        [ByName("Copy")]
        private readonly Button _buttonCopy;

        /// <summary>
        /// Properties as page object elements.
        /// Ability to search for generic control type.
        /// </summary>
        [Name("'Help' button")]
        [ByName("Help")]
        public Button ButtonHelp { get; set; }

        /// <summary>
        /// Simplified locator attribute.
        /// </summary>
        [Name("'Select' button")]
        [ByName("Select")]
        public Button ButtonSelect { get; set; }

        public Button ButtonCopy => _buttonCopy;

        /// <summary>
        /// Generic locator attribute.
        /// </summary>
        [Name("'Font' dropdown")]
        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts { get; set; }

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy { get; set; }

        /// <summary>
        /// Search for element directly without attributes.
        /// </summary>   
        public Checkbox CheckboxAdvancedView => Find<Checkbox>(ByLocator.Name("Advanced view"));

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet { get; set; }

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy { get; set; }

        /// <summary>
        /// Simplified locator attribute.
        /// </summary>
        [ById("133")]
        public WinControl ButtonSearch { get; set; }

        #endregion
    }
}
