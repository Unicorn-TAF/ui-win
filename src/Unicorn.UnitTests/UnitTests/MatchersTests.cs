using System.Collections.Generic;
using NUnit.Framework;
using Unicorn.Taf.Core.Testing.Verification.Matchers;
using static Unicorn.Taf.Core.Testing.Verification.Matchers.Is;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class MatchersTests
    {
        private readonly List<string> hasItemsA = new List<string>() { "qwerty", "qwerty12", "qwerty123" };
        private readonly List<string> hasItemsB = new List<string>() { "qwerty", "qwerty123" };
        private readonly List<string> hasItemsC = new List<string>() { "qwerty3", "qwerty1234" };
        private readonly List<string> hasItemsD = new List<string>() { "qwerty", "qwerty1234" };

        #region IsNull

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullPositive()
        {
            Taf.Core.Testing.Verification.Assert.That(null, Null());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotPositive()
        {
            Taf.Core.Testing.Verification.Assert.That("a", Not(Null()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate { Taf.Core.Testing.Verification.Assert.That("a", Null()); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate { Taf.Core.Testing.Verification.Assert.That(null, Not(Null())); });
        }

        #endregion

        #region IsEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringPositive()
        {
            Taf.Core.Testing.Verification.Assert.That("asd", EqualTo("asd"));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotPositive()
        {
            Taf.Core.Testing.Verification.Assert.That("asd", Not(EqualTo("asd1")));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberPositive()
        {
            Taf.Core.Testing.Verification.Assert.That(2, EqualTo(2));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberWithNotPositive()
        {
            Taf.Core.Testing.Verification.Assert.That(2, Not(EqualTo(3)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectPositive()
        {
            Taf.Core.Testing.Verification.Assert.That(new SampleObject(), EqualTo(new SampleObject()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotPositive()
        {
            Taf.Core.Testing.Verification.Assert.That(new SampleObject(), Not(EqualTo(new SampleObject("34", 324))));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That("asd", EqualTo("sd")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That("asd", Not(EqualTo("asd"))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(new SampleObject(), EqualTo(new SampleObject("ds", 234))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(new SampleObject(), Not(EqualTo(new SampleObject()))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, EqualTo("23")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullWithNotNegative()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, Not(EqualTo("23"))); });
        }

        #endregion

        #region "HasItems"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItems(hasItemsB));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive2()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItems(new[] { "qwerty" }));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive3()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItems(hasItemsA));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(hasItemsC)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive2()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(new[] { "qwerty6" })));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItems(hasItemsD)); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative2()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItems(new[] { "qwert12y" })); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullNegative3()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, Collection.HasItems(hasItemsB)); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(hasItemsB))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative2()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(new[] { "qwerty" }))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative3()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(hasItemsA))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative4()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItems(hasItemsD))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullWithNotNegative5()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, Not(Collection.HasItems(hasItemsB))); });
        }

        #endregion

        #region "HasItem"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItem("qwerty"));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItem("qwerty27")));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.HasItem("qwerty27")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNullNegative2()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, Collection.HasItem("qwerty")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.HasItem("qwerty"))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNullNegative2()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(
                delegate { Taf.Core.Testing.Verification.Assert.That(null, Not(Collection.HasItem("qwerty"))); });
        }

        #endregion

        #region "IsNullOrEmpty"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(null, Collection.IsNullOrEmpty());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive2()
        {
            Taf.Core.Testing.Verification.Assert.That(new List<string>(), Collection.IsNullOrEmpty());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive3()
        {
            Taf.Core.Testing.Verification.Assert.That(new string[0], Collection.IsNullOrEmpty());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate { Taf.Core.Testing.Verification.Assert.That(hasItemsA, Collection.IsNullOrEmpty()); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive1()
        {
            Taf.Core.Testing.Verification.Assert.That(hasItemsA, Not(Collection.IsNullOrEmpty()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive2()
        {
            Taf.Core.Testing.Verification.Assert.That(new int[2] { 2, 3 }, Not(Collection.IsNullOrEmpty()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotNegative1()
        {
            Assert.Throws<Taf.Core.Testing.Verification.AssertionException>(delegate { Taf.Core.Testing.Verification.Assert.That(null, Not(Collection.IsNullOrEmpty())); });
        }

        #endregion
    }
}