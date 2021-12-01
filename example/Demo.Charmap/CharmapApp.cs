using Demo.Charmap.Gui;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.PageObject;

namespace Demo.Charmap
{
    /// <summary>
    /// Describes desktop application (windows charmap application).
    /// should inherit <see cref="Application"/>.
    /// </summary>
    public class CharmapApp : Application
    {
        private static CharmapApp _charmap = null;

        /// <summary>
        /// Application constructor. Calls base constructor with path to application and application executable name.
        /// </summary>
        public CharmapApp() 
            : base(@"C:\Windows\System32\", "charmap.exe") 
        { 
        }

        /// <summary>
        /// It's possible to search for controls directly from application.
        /// </summary>
        [Find(Using.Name, "Character Map")]
        public CharmapWindow Window { get; set; }

        public static CharmapApp Charmap => 
            _charmap ?? (_charmap = new CharmapApp());
    }
}
