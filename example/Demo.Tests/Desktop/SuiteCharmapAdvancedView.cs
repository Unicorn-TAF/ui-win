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
    /// <summary>
    /// Desktop application test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Charmap Advanced View")]
    [Tag("Desktop"), Tag("Charmap"), Tag("Charmap.AdvancedView")]
    [Metadata(key: "Description", value: "Tests for Charmap Advanced View controls behavior")]
    [Metadata(key: "Specs link", value: "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapAdvancedView : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        /// <summary>
        /// Data for parameterized test. First parameter is <see cref="DataSet"/> name 
        /// and is not considered in parameterization.
        /// The method should be static.
        /// </summary>
        /// <returns></returns>
        public static List<DataSet> GetFontsData() =>
            new List<DataSet>
            {
                new DataSet("Calibri font", "Calibri", UI.Control.Enabled(), UI.Control.Enabled()),
                new DataSet("Consolas font", "Consolas", UI.Control.Enabled(), UI.Control.Enabled()),
                new DataSet("Courier font", "Courier", Is.Not(UI.Control.Enabled()), Is.Not(UI.Control.Enabled())),
                new DataSet("Wingdings font", "Wingdings", Is.Not(UI.Control.Enabled()), Is.Not(UI.Control.Enabled()))
            };

        /// <summary>
        /// Actions before whole suite execution.
        /// </summary>
        [BeforeSuite]
        public void ClassInit()
        {
            Do.UI.CharMap.StartApplication();
            Do.UI.CharMap.SelectFont("Cambria");
            Do.UI.CharMap.SetAdvancedView(true);
        }

        /// <summary>
        /// Example of test with execution order specified.
        /// </summary>
        [Order(1)]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Advanced View' dropdowns default values")]
        public void TestFontDropdownDefaultState()
        {
            Do.Assertion.AssertThat(Charmap.Window.DropdownCharacterSet, UI.Dropdown.HasSelectedValue("Unicode"));
            Do.Assertion.AssertThat(Charmap.Window.DropdownGroupBy, UI.Dropdown.HasSelectedValue("All"));
        }

        /// <summary>
        /// Example of parameterized test.
        /// Number of parameters should be the same as number of items in <see cref="DataSet"/> (DataSet name is not considered)
        /// </summary>
        /// <param name="font">1st DataSet parameter</param>
        /// <param name="matcher1">2nd DataSet parameter</param>
        /// <param name="matcher2">3rd DataSet parameter</param>
        [Order(2)]
        [Author("Vitaliy Dobriyan")]
        [Test("Check 'Advanced View' section controls enabled state")]
        [TestData(nameof(GetFontsData))]
        public void TestAdvancedViewSectionControlsEnabledState(string font, TypeSafeMatcher<IControl> matcher1, TypeSafeMatcher<IControl> matcher2)
        {
            Do.UI.CharMap.SelectFont(font);
            Do.Assertion.AssertThat(Charmap.Window.DropdownCharacterSet, matcher1);
            Do.Assertion.AssertThat(Charmap.Window.DropdownGroupBy, matcher2);
            Do.Assertion.AssertThat(Charmap.Window.ButtonSearch, Is.Not(UI.Control.Enabled()));
        }

        /// <summary>
        /// Actions after whole suite execution.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.UI.CharMap.CloseApplication();
    }
}
