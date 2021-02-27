using Demo.Charmap;
using Demo.Tests.Base;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Desktop
{
    [Suite("Charmap Advanced View")]
    [Tag("Desktop"), Tag("Charmap"), Tag("Charmap.AdvancedView")]
    [Metadata(key: "Description", value: "Tests for Charmap Advanced View controls behavior")]
    [Metadata(key: "Specs link", value: "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapAdvancedView : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        public static List<DataSet> GetFontsData() =>
            new List<DataSet>
            {
                new DataSet("Calibri font", "Calibri", UI.Control.Enabled(), UI.Control.Enabled()),
                new DataSet("Consolas font", "Consolas", UI.Control.Enabled(), UI.Control.Enabled()),
                new DataSet("Courier font", "Courier", Is.Not(UI.Control.Enabled()), Is.Not(UI.Control.Enabled())),
                new DataSet("Wingdings font", "Wingdings", Is.Not(UI.Control.Enabled()), Is.Not(UI.Control.Enabled()))
            };

        [BeforeSuite]
        public void ClassInit()
        {
            Do.UI.CharMap.StartApplication();
            Do.UI.CharMap.SelectFont("Cambria");
            Do.UI.CharMap.SetAdvancedView(true);
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Advanced View' dropdowns default values")]
        public void TestFontDropdownDefaultState()
        {
            Do.Assertion.AssertThat(Charmap.Window.DropdownCharacterSet, UI.Dropdown.HasSelectedValue("Unicode"));
            Do.Assertion.AssertThat(Charmap.Window.DropdownGroupBy, UI.Dropdown.HasSelectedValue("All"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Check 'Advanced View' section controls enabled state")]
        [TestData("GetFontsData")]
        public void TestAdvancedViewSectionControlsEnabledState(string font, TypeSafeMatcher<IControl> matcher1, TypeSafeMatcher<IControl> matcher2)
        {
            Do.UI.CharMap.SelectFont(font);
            Do.Assertion.AssertThat(Charmap.Window.DropdownCharacterSet, matcher1);
            Do.Assertion.AssertThat(Charmap.Window.DropdownGroupBy, matcher2);
            Do.Assertion.AssertThat(Charmap.Window.ButtonSearch, Is.Not(UI.Control.Enabled()));
        }

        [AfterSuite]
        public void ClassTearDown() =>
            Do.UI.CharMap.CloseApplication();
    }
}
