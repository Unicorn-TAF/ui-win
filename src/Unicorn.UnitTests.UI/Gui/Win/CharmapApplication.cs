﻿using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.Controls.Dynamic;
using Unicorn.UI.Win.PageObject;

namespace Unicorn.UnitTests.UI.Gui.Win
{
    public class CharmapApplication : Application
    {
        public CharmapApplication() : base(@"C:\Windows\System32", "charmap.exe")
        {
        }

        [Find(Using.Name, "Character Map")]
        public WindowCharMap Window { get; set; }

        [Find(Using.Name, "asdlkjfghsdhjkfgdsfkjhfg")]
        public WindowCharMap FakeWindow { get; set; }

        [Find(Using.Name, "Character Map")]
        [DefineDialog(DialogElement.Close, Using.Name, "Close")]
        public DynamicDialog WindowDynamic { get; set; }
    }
}
