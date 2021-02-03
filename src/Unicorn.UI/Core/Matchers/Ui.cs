using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Matchers.IControlMatchers;
using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Ui
    {
        /// <summary>
        /// Entry point for general control matchers.
        /// </summary>
        public static class Control
        {
            /// <summary>
            /// Matcher to check if UI control has specified attribute which contains expected value.
            /// </summary>
            /// <param name="attribute">attribute name</param>
            /// <param name="expectedValue">expected part of attribute value</param>
            /// <returns><see cref="AttributeContainsMatcher"/> instance</returns>
            public static AttributeContainsMatcher HasAttributeContains(string attribute, string expectedValue)
                => new AttributeContainsMatcher(attribute, expectedValue);

            /// <summary>
            /// Matcher to check if UI control has specified attribute with specified value.
            /// </summary>
            /// <param name="attribute">attribute name</param>
            /// <param name="expectedValue">expected attribute value</param>
            /// <returns><see cref="AttributeIsEqualToMatcher"/> instance</returns>
            public static AttributeIsEqualToMatcher HasAttributeIsEqualTo(string attribute, string expectedValue)
                => new AttributeIsEqualToMatcher(attribute, expectedValue);

            /// <summary>
            /// Matcher to check if  UI control is enabled.
            /// </summary>
            /// <returns><see cref="ControlEnabledMatcher"/> instance</returns>
            public static ControlEnabledMatcher Enabled()
                => new ControlEnabledMatcher();

            /// <summary>
            /// Matcher to check if UI control is visible.
            /// </summary>
            /// <returns><see cref="ControlVisibleMatcher"/> instance</returns>
            public static ControlVisibleMatcher Visible()
                => new ControlVisibleMatcher();

            /// <summary>
            /// Matcher to check if <see cref="ISelectable"/> UI control is selected.
            /// </summary>
            /// <returns><see cref="SelectedMatcher"/> instance</returns>
            public static SelectedMatcher Selected()
                => new SelectedMatcher();

            /// <summary>
            /// Matcher to check if <see cref="ISortable"/> UI control is sorted by specified direction.
            /// </summary>
            /// <param name="direction">sorting direction</param>
            /// <returns><see cref="SortedMatcher"/> instance</returns>
            public static SortedMatcher Sorted(SortDirection direction)
                => new SortedMatcher(direction);

            /// <summary>
            /// Matcher to check if UI control has specific text.
            /// </summary>
            /// <param name="expectedTextRegex">expected control text</param>
            /// <returns><see cref="ControlHasTextMatcher"/> instance</returns>
            public static ControlHasTextMatcher HasText(string expectedText)
                => new ControlHasTextMatcher(expectedText);

            /// <summary>
            /// Matcher to check if UI control has text matching specific regular expression.
            /// </summary>
            /// <param name="expectedTextRegex">regular expression</param>
            /// <returns><see cref="ControlHasTextMatchesMatcher"/> instance</returns>
            public static ControlHasTextMatchesMatcher HasTextMatching(string expectedTextRegex)
                => new ControlHasTextMatchesMatcher(expectedTextRegex);
        }

        /// <summary>
        /// Entry point for checkbox matchers.
        /// </summary>
        public static class Checkbox
        {
            /// <summary>
            /// Gets matcher to check if checkbox is checked.
            /// </summary>
            /// <returns>matcher instance</returns>
            public static CheckboxCheckedMatcher Checked()
                => new CheckboxCheckedMatcher();

            /// <summary>
            /// Gets matcher to check if checkbox has desired check state.
            /// </summary>
            /// <returns>matcher instance</returns>
            public static CheckboxHasCheckStateMatcher HasCheckState(bool isChecked)
                => new CheckboxHasCheckStateMatcher(isChecked);
        }

        /// <summary>
        /// Entry point for DataGrid matchers.
        /// </summary>
        public static class DataGrid
        {
            /// <summary>
            /// Gets matcher to check if data grid contains row with specified value in specified column.
            /// </summary>
            /// <param name="column">column to search by</param>
            /// <param name="cellValue">expected cell value</param>
            /// <returns>matcher instance</returns>
            public static DataGridHasRowMatcher HasRow(string column, string cellValue)
                => new DataGridHasRowMatcher(column, cellValue);

            /// <summary>
            /// Gets matcher to check if data grid contains specified column.
            /// </summary>
            /// <param name="columnName">expected data grid column</param>
            /// <returns>matcher instance</returns>
            public static DataGridHasColumnMatcher HasColumn(string columnName)
                => new DataGridHasColumnMatcher(columnName);
        }

        /// <summary>
        /// Entry point for dropdown matchers.
        /// </summary>
        public static class Dropdown
        {
            /// <summary>
            /// Gets matcher to check if dropdown contains specified item.
            /// </summary>
            /// <param name="expectedValue">expected dropdown item</param>
            /// <returns>matcher instance</returns>
            public static DropdownHasSelectedValueMatcher HasSelectedValue(string expectedValue)
                => new DropdownHasSelectedValueMatcher(expectedValue);

            /// <summary>
            /// Gets matcher to check if dropdown is expanded.
            /// </summary>
            /// <returns>matcher instance</returns>
            public static DropdownExpandedMatcher Expanded()
                => new DropdownExpandedMatcher();
        }

        /// <summary>
        /// Entry point for Textinput matchers.
        /// </summary>
        public static class Input
        {
            /// <summary>
            /// Gets matcher to check if Textinput has specified value.
            /// </summary>
            /// <param name="expectedValue">expected value</param>
            /// <returns>matcher instance</returns>
            public static InputHasValueMatcher HasValue(string expectedValue)
                => new InputHasValueMatcher(expectedValue);
        }
    }
}
