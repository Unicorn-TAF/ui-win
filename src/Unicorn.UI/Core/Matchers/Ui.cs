namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for UI matchers
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// Entry point for general control matchers.
        /// </summary>
        public static ControlMatchers Control =>
            new ControlMatchers();

        /// <summary>
        /// Entry point for checkbox matchers.
        /// </summary>
        public static CheckboxMatchers Checkbox =>
            new CheckboxMatchers();

        /// <summary>
        /// Entry point for DataGrid matchers.
        /// </summary>
        public static DataGridMatchers DataGrid =>
            new DataGridMatchers();

        /// <summary>
        /// Entry point for dropdown matchers.
        /// </summary>
        public static DropdownMatchers Dropdown =>
            new DropdownMatchers();

        /// <summary>
        /// Entry point for Textinput matchers.
        /// </summary>
        public static TextInputMatchers TextInput =>
            new TextInputMatchers();

        /// <summary>
        /// Entry point for Textinput matchers.
        /// </summary>
        public static WindowMatchers Window =>
            new WindowMatchers();
    }
}
