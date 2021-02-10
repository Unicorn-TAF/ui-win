using NUnit.Framework;
using Unicorn.UI.Core.Matchers;
using Unicorn.UnitTests.Gui.Win;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class UiMatchers
    {
        private static CharmapApplication charmap;

        [OneTimeSetUp]
        public static void Setup()
        {
            charmap = new CharmapApplication();
            charmap.Start();
        }

        [OneTimeTearDown]
        public static void TearDown() =>
            charmap.Close();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Visible matcher (positive)")]
        public void TestVisibleMatcherPositive() =>
            Uv.Assert.That(charmap.Window, Ui.Control.Visible());

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Visible matcher with negation (positive)")]
        public void TestVisibleMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.DropdownTextInputFonts, Um.Is.Not(Ui.Control.Visible()));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Visible matcher for null")]
        public void TestVisibleMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Control.Visible());
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Enabled matcher (positive)")]
        public void TestEnabledMatcherPositive() =>
            Uv.Assert.That(charmap.Window.ButtonHelp, Ui.Control.Enabled());

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Enabled matcher for null")]
        public void TestEnabledMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
                {
                    Uv.Assert.That(null, Ui.Control.Enabled());
                });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Enabled matcher with negation (positive)")]
        public void TestEnabledMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.ButtonCopy, Um.Is.Not(Ui.Control.Enabled()));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasText matcher (positive)")]
        public void TestHasTextMatcherPositive() =>
            Uv.Assert.That(charmap.Window.ButtonHelp, Ui.Control.HasText("Help"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasText matcher for null")]
        public void TestHasTextMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Control.HasText(""));
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasText matcher with negation (positive)")]
        public void TestHasTextMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.ButtonCopy, Um.Is.Not(Ui.Control.HasText("sdf")));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasTextMatching matcher (positive)")]
        public void TestHasTextMatchingMatcherPositive() =>
            Uv.Assert.That(charmap.Window.ButtonHelp, Ui.Control.HasTextMatching("H[A-z]+p"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasTextMatching matcher for null")]
        public void TestHasTextMatchingMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Control.HasTextMatching(""));
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasTextMatching matcher with negation (positive)")]
        public void TestHasTextMatchingMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.ButtonCopy, Um.Is.Not(Ui.Control.HasTextMatching("H[A-a]+p")));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeContains matcher (positive)")]
        public void TestHasAttributeContainsMatcherPositive() =>
            Uv.Assert.That(charmap.Window.DropdownFonts, Ui.Control.HasAttributeContains("class", "Box"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeContains matcher for null")]
        public void TestHasAttributeContainsMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Control.HasAttributeContains("", ""));
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeContains matcher with negation (positive)")]
        public void TestHasAttributeContainsMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.DropdownFonts, Um.Is.Not(Ui.Control.HasAttributeContains("class", "sdf")));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeIsEqualTo matcher (positive)")]
        public void TestHasAttributeIsEqualToMatcherPositive() =>
            Uv.Assert.That(charmap.Window.DropdownFonts, Ui.Control.HasAttributeIsEqualTo("class", "ComboBox"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeIsEqualTo matcher with negation (positive)")]
        public void TestHasAttributeIsEqualToMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.DropdownFonts, Um.Is.Not(Ui.Control.HasAttributeIsEqualTo("class", "Box")));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "HasAttributeIsEqualTo matcher for null")]
        public void TestHasAttributeIsEqualToMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Control.HasAttributeIsEqualTo("", ""));
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "DropdownExpanded matcher (positive)")]
        public void TestDropdownExpandedMatcherPositive()
        {
            charmap.Window.DropdownFonts.Expand();
            Uv.Assert.That(charmap.Window.DropdownFonts, Ui.Dropdown.Expanded());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "DropdownExpanded matcher with negation (positive)")]
        public void TestDropdownExpandedMatcherWithNegationPositive()
        {
            charmap.Window.DropdownFonts.Collapse();
            Uv.Assert.That(charmap.Window.DropdownFonts, Um.Is.Not(Ui.Dropdown.Expanded()));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "DropdownExpanded matcher for null")]
        public void TestDropdownExpandedMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Dropdown.Expanded());
            });

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "InputHasValue matcher (positive)")]
        public void TestInputHasValueMatcherPositive() =>
            Uv.Assert.That(charmap.Window.InputCharactersToCopy, Ui.Input.HasValue(""));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "InputHasValue matcher with negation (positive)")]
        public void TestInputHasValueMatcherWithNegationPositive() =>
            Uv.Assert.That(charmap.Window.InputCharactersToCopy, Um.Is.Not(Ui.Input.HasValue("somevalue")));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "InputHasValue matcher for null")]
        public void TestInputHasValueMatcherNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Ui.Input.HasValue(""));
            });
    }
}
