using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;
using Demo.AndroidDialer;
using Demo.Tests.Base;

namespace Demo.Tests.Android
{
    [Disabled("Android emulator is not configured")]
    [Suite("Tests Android dialer application")]
    [Tag("Android"), Tag("Dialer"), Tag("Dialer.Dialpad")]
    [Metadata(key: "Description", value: "Sample suite containing tests for android Dialer app")]
    [Metadata(key: "Target device", value: "Android emulator")]
    public class SuiteAndroidDialer : BaseTestSuite
    {
        AndroidDialerApp Dialer => AndroidDialerApp.Instance;

        [BeforeTest]
        public void TestInit() =>
            Do.UI.Android.OpenDialer();

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check dialpad button")]
        public void TestDialpadButton()
        {
            Do.UI.Android.ClickDialpadButton();
            Do.Assertion.AssertThat(Dialer.App.ActionBar.ButtonDial, UI.Control.Visible());
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check ability to type call number")]
        public void TestAbilityToTypeCallNumber()
        {
            Do.UI.Android.ClickDialpadButton();
            Do.UI.Android.TapNumber("one");
            Do.UI.Android.TapNumber("two");
            Do.UI.Android.TapNumber("three");
            Do.Assertion.AssertThat(Dialer.App.MainFrame.InputNumber, UI.Control.HasText("1 23"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Check calls history")]
        public void TestCallsHistory()
        {
            Do.UI.Android.ClickDialpadButton();
            Do.UI.Android.OpenCallsHistory();
        }

        [AfterTest]
        public void TestCleanup() =>
            Do.UI.Android.CloseDialer();
    }
}
