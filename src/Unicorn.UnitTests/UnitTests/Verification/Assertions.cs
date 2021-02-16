using NUnit.Framework;
using Unicorn.UnitTests.BO;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Verification
{
    [TestFixture]
    public class Assertions
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void TestSoftAssertion()
        {
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                new Uv.ChainAssert()
                    .That("asd", Um.Is.EqualTo("asd"))
                    .That(2, Um.Is.EqualTo(2))
                    .That(new SampleObject(), Um.Is.EqualTo(new SampleObject("ds", 234)))
                    .That(new SampleObject(), Um.Is.EqualTo(new SampleObject()))
                    .AssertChain();
            });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertion()
        {
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That("as2d", Um.Is.EqualTo("asd"));
            });
        }
    }
}
