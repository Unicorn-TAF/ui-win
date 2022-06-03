using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Dynamic;
using Unicorn.UI.Win.Controls.Typified;

namespace Unicorn.UnitTests.UI.Gui.Win
{
    public class WindowCharMap : Window
    {
        private readonly CopyButtonWithDefaultLocator _buttonCopyDefaultLocator;

        [Find(Using.Name, "Copy")]
        private readonly Button _buttonCopyAsField;

        [Find(Using.Name, "Copy")]
        public Button ButtonCopy { get; set; }

        [Find(Using.Name, "Font :")]
        public Dropdown DropdownFonts { get; set; }

        public Text HiddenText => DropdownFonts.Find<Text>(ByLocator.Name("Font :"));

        [Find(Using.Name, "Font :")]
        public TextInput DropdownTextInputFonts { get; set; }

        [Find(Using.Name, "Characters to copy :")]
        public TextInput InputCharactersToCopy { get; set; }

        [Find(Using.Name, "Advanced view")]
        public Checkbox CheckboxAdvancedView { get; set; }

        public CopyButtonWithDefaultLocator ButtonCopyDefaultLocator { get; set; }

        public CopyButtonWithDefaultLocator ButtonCopyDefaultLocatorGetter => _buttonCopyDefaultLocator;

        #region "Advanced view"

        [Find(Using.Name, "Character set :")]
        public Dropdown DropdownCharacterSet { get; set; }

        [Find(Using.Name, "Group by :")]
        public Dropdown DropdownGroupBy { get; set; }

        [Find(Using.Id, "133")]
        public Button ButtonSearch { get; set; }

        #endregion

        [ByName("Select")]
        public Button ButtonSelectLocatedByName { get; set; }

        public Button SelectButton => ButtonSelect;

        public Button ButtonHelp => Find<Button>(ByLocator.Name("Help"));

        [Find(Using.Id, "100")]
        public WinControl ButtonHelpGeneric { get; set; }

        [Find(Using.Name, "Select")]
        protected Button ButtonSelect { get; set; }

        public Button GetCopyButtonFromField() => _buttonCopyAsField;

        [ById("105")]
        [DefineDropdown(DropdownElement.ValueInput, Using.Name, "Font :")]
        [DefineDropdown(DropdownElement.ExpandCollapse, Using.Id, "DropDown")]
        [DefineDropdown(DropdownElement.OptionsFrame, Using.Class, "ComboLBox")]
        [DefineDropdown(DropdownElement.Option, Using.Class, "")]
        public DynamicDropdown DDropdown { get; set; }

        [ById("105")]
        [DefineDropdown(DropdownElement.ExpandCollapse, Using.Id, "DropDown")]
        [DefineDropdown(DropdownElement.OptionsFrame, Using.Class, "ComboLBox")]
        [DefineDropdown(DropdownElement.Option, Using.Class, "")]
        public DynamicDropdown DDropdownNoInput { get; set; }
    }
}
