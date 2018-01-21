using ProjectSpecific.UI.Gui;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Driver;

namespace ProjectSpecific.UI
{
    public static class Desktop
    {
        public static WindowCharMap CharMap => GuiDriver.Instance.Find<WindowCharMap>(ByLocator.Name("Character Map"));
    }
}
