using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.PageObject;

namespace Unicorn.UnitTests.Gui.Desktop
{
    public class CharmapApplication : Application
    {
        public CharmapApplication(string path, string exeName) : base(path, exeName)
        {
        }

        [Find(Using.Name, "Character Map")]
        public WindowCharMap Window { get; set; }

        [Find(Using.Name, "asdlkjfghsdhjkfgdsfkjhfg")]
        public WindowCharMap FakeWindow { get; set; }
    }
}
