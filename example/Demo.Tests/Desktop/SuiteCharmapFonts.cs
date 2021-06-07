using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;
using Unicorn.Taf.Core.Verification.Matchers;
using Demo.Charmap;
using Demo.Tests.Base;

namespace Demo.Tests.Desktop
{
    [Suite("Charmap Fonts dropdown")]
    [Tag("Desktop"), Tag("Charmap"), Tag("Charmap.Fonts")]
    [Metadata(key: "Description", value: "Tests for Charmap fonts dropdown functionality")]
    [Metadata(key: "Specs link", value: "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapFonts : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        public static List<DataSet> GetFontsData() =>
            new List<DataSet>
            {
                new DataSet("Calibri font", "Calibri"),
                new DataSet("Consolas font", "Consolas"),
                new DataSet("Courier font", "Courier"),
                new DataSet("Wingdings font", "Wingdings")
            };

        [BeforeSuite]
        public void ClassInit() =>
            Do.UI.CharMap.StartApplication();

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Font' dropdown default state")]
        public void TestFontDropdownDefaultState()
        {
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, Is.Not(UI.Control.Visible()));
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, UI.Control.Enabled());
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Font' dropdown ability to select")]
        [TestData(nameof(GetFontsData))]
        public void TestFontDropdownAbilityToSelect(string font)
        {
            Do.UI.CharMap.SelectFont(font);
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, UI.Dropdown.HasSelectedValue(font));
        }

        [AfterSuite]
        public void ClassTearDown() =>
            Do.UI.CharMap.CloseApplication();
    }
}
