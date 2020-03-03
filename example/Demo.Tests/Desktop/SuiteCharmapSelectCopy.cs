using Demo.Charmap;
using Demo.Tests.Base;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Desktop
{
    [Suite("Charmap Select/Copy")]
    [Tag("Desktop"), Tag("Charmap"), Tag("Charmap.SelectCopy")]
    [Metadata(key: "Description", value: "Tests for Charmap select/copy functionality")]
    [Metadata(key: "Specs link", value: "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapSelectCopy : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        [BeforeTest]
        public void TestInit() =>
            Do.UI.CharMap.StartApplication();

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check Select & Copy buttons default state")]
        public void TestSelectCopyButtonsDefalutState()
        {
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, Control.Enabled());

            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, Is.Not(Control.Enabled()));
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check 'Copy' button is enabled after selection")]
        public void TestCopyIsEnabledAfterSelection()
        {
            Do.UI.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, Control.Enabled());
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check ability to select chars")]
        public void TestAbilityToSelectChars()
        {
            Do.UI.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.InputCharactersToCopy, Is.Not(Control.HasAttribute("text").IsEqualTo("")));
        }

        [AfterTest]
        public void TestTearDown() =>
            Do.UI.CharMap.CloseApplication();
    }
}
