using NUnit.Framework;
using ProjectSpecific.BO;
using Unicorn.Core.Testing.Assertions;
using static Unicorn.Core.Testing.Assertions.CoreMatchers;

namespace Tests.UnitTests
{
    [TestFixture]
    public class AssertionsTest
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void IsNullMatcherPositiveTest()
        {
            Assert.Throws<AssertionError>(delegate 
            {
                SoftAssertion assert = new SoftAssertion();
                assert.AssertThat("asd", IsEqualTo("asd"))
                    .AssertThat(2, IsEqualTo(2))
                    .AssertThat(new SampleObject(), IsEqualTo(new SampleObject("ds", 234)))
                    .AssertThat(new SampleObject(), IsEqualTo(new SampleObject()))
                    .AssertThat("bla-bla-bla message", new SampleObject(), IsEqualTo(23));

                assert.AssertAll();
            });
        }
    }
}
