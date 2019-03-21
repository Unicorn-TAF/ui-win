using NUnit.Framework;
using Unicorn.Taf.Core.Testing.Verification;
using static Unicorn.Taf.Core.Testing.Verification.Matchers.Is;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class AssertionsTest
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void TestSoftAssertion()
        {
            NUnit.Framework.Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate 
            {
                Verify assert = new Verify();
                assert.VerifyThat("asd", EqualTo("asd"))
                    .VerifyThat(2, EqualTo(2))
                    .VerifyThat(new SampleObject(), EqualTo(new SampleObject("ds", 234)))
                    .VerifyThat(new SampleObject(), EqualTo(new SampleObject()));

                assert.AssertAll();
            });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertion()
        {
            NUnit.Framework.Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate
            {
                Taf.Core.Testing.Verification.Assert.That("as2d", EqualTo("asd"));
            });
        }
    }
}
