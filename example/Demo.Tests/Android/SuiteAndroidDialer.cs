using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;
using Demo.AndroidDialer;
using Demo.Tests.Base;
using Unicorn.UI.Core.PageObject;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Demo.Tests.Android
{
    //[Disabled("Android emulator is not configured")]
    [Suite("Tests Android dialer application")]
    [Tag("Android"), Tag("Dialer"), Tag("Dialer.Dialpad")]
    [Metadata(key: "Description", value: "Suite with tests for android Dialer app")]
    [Metadata(key: "Target device", value: "Android emulator")]
    [Metadata(key: "API version", value: "v25")]
    public class SuiteAndroidDialer : BaseTestSuite
    {
        private AndroidDialerApi25 dialer;

        [BeforeTest]
        public void TestInit() =>
            dialer = Do.UI.Android.OpenDialer();

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check dialpad button")]
        public void TestDialpadButton()
        {
            Do.Assertion.AssertThat(dialer.AppFrame.DialPadButton, UI.Control.Visible());
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check ability to type call number")]
        public void TestAbilityToTypeCallNumber()
        {
            Do.UI.Android.OpenDialpad();
            Do.UI.Android.TapNumber("#");
            Do.UI.Android.TapNumber("1");
            Do.UI.Android.TapNumber("2");
            Do.UI.Android.TapNumber("3");
            Do.Assertion.AssertThat(dialer.AppFrame.DialPad.NumberText, UI.Control.HasText("#123"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Check calls history")]
        public void TestCallsHistory()
        {
            Do.UI.Android.OpenCallsHistory();
            Do.Assertion.AssertThat(
                dialer.AppFrame.CallsHistory.EmptyListIcon.ExistsInPageObject(), 
                Is.EqualTo(true));
        }

        [AfterTest]
        public void TestCleanup() =>
            Do.UI.Android.CloseDialer();
    }
}
