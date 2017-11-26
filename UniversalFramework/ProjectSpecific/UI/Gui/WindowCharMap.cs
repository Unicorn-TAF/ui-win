using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace ProjectSpecific.UI.Gui
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
    }
}
