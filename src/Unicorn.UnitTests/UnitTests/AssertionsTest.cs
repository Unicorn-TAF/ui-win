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
                new ChainAssert()
                    .That("asd", EqualTo("asd"))
                    .That(2, EqualTo(2))
                    .That(new SampleObject(), EqualTo(new SampleObject("ds", 234)))
                    .That(new SampleObject(), EqualTo(new SampleObject()))
                    .AssertChain();
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
