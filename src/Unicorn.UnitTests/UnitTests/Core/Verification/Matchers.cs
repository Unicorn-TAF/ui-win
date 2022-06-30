using NUnit.Framework;
using Unicorn.UnitTests.BO;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Core.Verification
{
    [TestFixture]
    public class Matchers
    {
        private readonly string _compare1 = "compare1";
        private readonly string _compare2 = "compare2";

        private readonly int _intCompare1 = 1;
        private readonly int _intCompare2 = 2;

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

        #region IsGreaterThan

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanPositive() =>
            Uv.Assert.That(_compare2, Um.Is.IsGreaterThan(_compare1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanWithNotPositive() =>
            Uv.Assert.That(_compare1, Um.Is.Not(Um.Is.IsGreaterThan(_compare2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_compare1, Um.Is.IsGreaterThan(_compare2));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_compare1, Um.Is.IsGreaterThan(_compare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanWithNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.IsGreaterThan(_compare2));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanWithNullWithNot() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.IsGreaterThan(_compare1)));
            });

        #endregion

        #region IsGreaterThanOrEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanOrEqualToPositive() =>
            Uv.Assert.That(_intCompare2, Um.Is.IsGreaterThanOrEqualTo(_intCompare1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanOrEqualToPositive1() =>
            Uv.Assert.That(_intCompare1, Um.Is.IsGreaterThanOrEqualTo(_intCompare1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanOrEqualToNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_intCompare1, Um.Is.IsGreaterThanOrEqualTo(_intCompare2));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanOrEqualToWithNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.IsGreaterThanOrEqualTo(_intCompare2));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsGreaterThanOrEqualToWithNullWithNot() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.IsGreaterThanOrEqualTo(_intCompare2)));
            });

        #endregion

        #region IsLessThan

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanPositive() =>
            Uv.Assert.That(_compare1, Um.Is.IsLessThan(_compare2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanWithNotPositive() =>
            Uv.Assert.That(_compare2, Um.Is.Not(Um.Is.IsLessThan(_compare1)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_compare2, Um.Is.IsLessThan(_compare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_compare1, Um.Is.IsLessThan(_compare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanWithNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.IsLessThan(_compare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanWithNullWithNot() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.IsLessThan(_compare1)));
            });

        #endregion

        #region IsGreaterThanOrEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanOrEqualToPositive() =>
            Uv.Assert.That(_intCompare1, Um.Is.IsLessThanOrEqualTo(_intCompare2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanOrEqualToPositive1() =>
            Uv.Assert.That(_intCompare2, Um.Is.IsLessThanOrEqualTo(_intCompare2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanOrEqualToNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(_intCompare2, Um.Is.IsLessThanOrEqualTo(_intCompare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanOrEqualToWithNull() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.IsLessThanOrEqualTo(_intCompare1));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsLessThanOrEqualToWithNullWithNot() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(null, Um.Is.Not(Um.Is.IsLessThanOrEqualTo(_intCompare1)));
            });

        #endregion
    }
}