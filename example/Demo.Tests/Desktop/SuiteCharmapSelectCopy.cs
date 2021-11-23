using Demo.Charmap;
using Demo.Tests.Base;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Desktop
{
    /// <summary>
    /// Desktop application test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Charmap Select/Copy")]
    [Tag("Desktop"), Tag("Charmap"), Tag("Charmap.SelectCopy")]
    [Metadata(key: "Description", value: "Tests for Charmap select/copy functionality")]
    [Metadata(key: "Specs link", value: "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapSelectCopy : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        /// <summary>
        /// Actions executed before each test.
        /// </summary>
        [BeforeTest]
        public void TestInit() =>
            Do.UI.CharMap.StartApplication();

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check Select & Copy buttons default state")]
        public void TestSelectCopyButtonsDefalutState()
        {
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, UI.Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, UI.Control.Enabled());

            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, UI.Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, Is.Not(UI.Control.Enabled()));
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Copy' button is enabled after selection")]
        public void TestCopyIsEnabledAfterSelection()
        {
            Do.UI.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, UI.Control.Enabled());
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check ability to select chars")]
        public void TestAbilityToSelectChars()
        {
            Do.UI.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.InputCharactersToCopy, Is.Not(UI.Control.HasText(string.Empty)));
        }

        /// <summary>
        /// Actions executed after each test.
        /// </summary>
        [AfterTest]
        public void TestTearDown() =>
            Do.UI.CharMap.CloseApplication();
    }
}
