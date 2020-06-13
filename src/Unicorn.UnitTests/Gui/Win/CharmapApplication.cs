using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.PageObject;

namespace Unicorn.UnitTests.Gui.Win
{
    public class CharmapApplication : Application
    {
        public CharmapApplication() : base(@"C:\Windows\System32", "charmap.exe")
        {
        }

        [Find(Using.Name, "Character Map")]
        public WindowCharMap Window { get; set; }
    }
}
