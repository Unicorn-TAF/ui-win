using NUnit.Framework;
using Unicorn.UnitTests.BO;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Verification
{
    [TestFixture]
    public class Matchers
    {
        #region IsNull

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullPositive() =>
            Uv.Assert.That(null, Um.Is.Null());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotPositive() =>
            Uv.Assert.That("a", Um.Is.Not(Um.Is.Null()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That("a", Um.Is.Null());
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.Null()));
            });

        #endregion

        #region IsEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringPositive() =>
            Uv.Assert.That("asd", Um.Is.EqualTo("asd"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotPositive() =>
            Uv.Assert.That("asd", Um.Is.Not(Um.Is.EqualTo("asd1")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberPositive() =>
            Uv.Assert.That(2, Um.Is.EqualTo(2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberWithNotPositive() =>
            Uv.Assert.That(2, Um.Is.Not(Um.Is.EqualTo(3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectPositive() =>
            Uv.Assert.That(new SampleObject(), Um.Is.EqualTo(new SampleObject()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotPositive() =>
            Uv.Assert.That(new SampleObject(), Um.Is.Not(Um.Is.EqualTo(new SampleObject("34", 324))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
                {
                    Uv.Assert.That("asd", Um.Is.EqualTo("sd"));
                });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That("asd", Um.Is.Not(Um.Is.EqualTo("asd")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(new SampleObject(), Um.Is.EqualTo(new SampleObject("ds", 234)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(new SampleObject(), Um.Is.Not(Um.Is.EqualTo(new SampleObject())));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.EqualTo("23"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullWithNotNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.EqualTo("23")));
            });

        #endregion
    }
}