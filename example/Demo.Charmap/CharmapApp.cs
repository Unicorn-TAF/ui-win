using Demo.Charmap.Gui;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.PageObject;

namespace Demo.Charmap
{
    public class CharmapApp : Application
    {
        private static CharmapApp _charmap = null;

        public CharmapApp() 
            : base(@"C:\Windows\System32\", "charmap.exe") 
        { 
        }

        [Find(Using.Name, "Character Map")]
        public CharmapWindow Window { get; set; }

        public static CharmapApp Charmap => 
            _charmap ?? (_charmap = new CharmapApp());
    }
}
