using System.Collections.Generic;
using NUnit.Framework;
using Unicorn.Taf.Core.Verification.Matchers;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class CollectionMatchers
    {
        #region Data
        private readonly string[] hasItemsA = new [] 
        {
            "qwerty", "qwerty12", "qwerty123"
        };

        private readonly string[] hasItemsB = new [] 
        {
            "qwerty", "qwerty123"
        };

        private readonly string[] hasItemsC = new [] 
        {
            "qwerty3", "qwerty1234"
        };

        private readonly string[] hasItemsD = new [] 
        {
            "qwerty", "qwerty1234"
        };

        private readonly string[] expected1 = new[]
            {
                "qwerty"
            };

        private readonly string[] expected2 = new[]
            {
                "qwerty6"
            };

        private readonly string[] expected3 = new[]
            {
                "qwert12y"
            };

        private readonly int[] actual1 = new[]
            {
                2,
                3
            };

        #endregion

        #region "HasItems"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive1() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsB));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive2() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(expected1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive3() =>
            Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsA));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsC)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive2() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(expected2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItems(hasItemsD));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItems(expected3));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullNegative3() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Collection.HasItems(hasItemsB));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsB)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(expected1)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative3() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsA)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative4() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItems(hasItemsD)));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullWithNotNegative5() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.HasItems(hasItemsB)));
            });

        #endregion

        #region "HasItem"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemPositive1() =>
            Uv.Assert.That(hasItemsA, Collection.HasItem("qwerty"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItem("qwerty27")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.HasItem("qwerty27"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Collection.HasItem("qwerty"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.HasItem("qwerty")));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNullNegative2() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.HasItem("qwerty")));
            });

        #endregion

        #region "IsNullOrEmpty"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive1() =>
            Uv.Assert.That(null, Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive2() =>
            Uv.Assert.That(new List<string>(), Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive3() =>
            Uv.Assert.That(new string[0], Collection.IsNullOrEmpty());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(hasItemsA, Collection.IsNullOrEmpty());
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive1() =>
            Uv.Assert.That(hasItemsA, Um.Is.Not(Collection.IsNullOrEmpty()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive2() =>
            Uv.Assert.That(actual1, Um.Is.Not(Collection.IsNullOrEmpty()));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotNegative1() =>
            Assert.Throws<Uv.AssertionException>(delegate 
            {
                Uv.Assert.That(null, Um.Is.Not(Collection.IsNullOrEmpty()));
            });

        #endregion
    }
}