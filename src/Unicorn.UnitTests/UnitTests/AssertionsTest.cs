using NUnit.Framework;
using Unicorn.Taf.Core.Verification;
using static Unicorn.Taf.Core.Verification.Matchers.Is;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class AssertionsTest
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void TestSoftAssertion()
        {
            NUnit.Framework.Assert.Throws<Taf.Core.Verification.AssertionException>(delegate 
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
            NUnit.Framework.Assert.Throws<Taf.Core.Verification.AssertionException>(delegate
            {
                Taf.Core.Verification.Assert.That("as2d", EqualTo("asd"));
            });
        }
    }
}
