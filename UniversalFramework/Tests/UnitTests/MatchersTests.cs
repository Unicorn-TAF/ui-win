using NUnit.Framework;
using ProjectSpecific.BO;
using System.Collections.Generic;
using Unicorn.Core.Testing.Assertions;
using static Unicorn.Core.Testing.Assertions.CollectionsMatchers;
using static Unicorn.Core.Testing.Assertions.CoreMatchers;

namespace Tests.UnitTests
{
    [TestFixture]
    public class MatchersTests
    {

        #region IsNull

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullPositive()
        {
            Assertion.AssertThat(null, IsNull());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotPositive()
        {
            Assertion.AssertThat("a", Not(IsNull()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullNegative()
        {
            Assert.Throws<AssertionError>( delegate { Assertion.AssertThat("a", IsNull()); } );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullWithNotNegative()
        {
            Assert.Throws<AssertionError>( delegate { Assertion.AssertThat(null, Not(IsNull())); } );
        }

        #endregion


        #region IsEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringPositive()
        {
            Assertion.AssertThat("asd", IsEqualTo(("asd")));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotPositive()
        {
            Assertion.AssertThat("asd", Not(IsEqualTo(("asd1"))));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberPositive()
        {
            Assertion.AssertThat(2, IsEqualTo((2)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNumberWithNotPositive()
        {
            Assertion.AssertThat(2, Not(IsEqualTo((3))));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectPositive()
        {
            Assertion.AssertThat(new SampleObject(), IsEqualTo(new SampleObject()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotPositive()
        {
            Assertion.AssertThat(new SampleObject(), Not(IsEqualTo(new SampleObject("34", 324))));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat("asd", IsEqualTo(("sd"))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToStringWithNotNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat("asd", Not(IsEqualTo(("asd")))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(new SampleObject(), IsEqualTo(new SampleObject("ds", 234))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToCustomObjectWithNotNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(new SampleObject(), Not(IsEqualTo(new SampleObject()))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNotCastableNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(new SampleObject(), IsEqualTo(23)); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNotCastableWithNotPositive()
        {
            Assertion.AssertThat(new SampleObject(), Not(IsEqualTo(23)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, IsEqualTo(23)); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsEqualToNullWithNotNegative()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, Not(IsEqualTo(23))); }
            );
        }

        #endregion



        List<string> hasItemsA = new List<string>() {
            "qwerty", "qwerty12", "qwerty123" };

        List<string> hasItemsB = new List<string>() {
            "qwerty", "qwerty123" };

        List<string> hasItemsC = new List<string>() {
            "qwerty3", "qwerty1234" };

        List<string> hasItemsD = new List<string>() {
            "qwerty", "qwerty1234" };

        #region "HasItems"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive1()
        {
            Assertion.AssertThat(hasItemsA, HasItems(hasItemsB));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive2()
        {
            Assertion.AssertThat(hasItemsA, HasItems(new[] { "qwerty" }));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsPositive3()
        {
            Assertion.AssertThat(hasItemsA, HasItems(hasItemsA));
        }



        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive1()
        {
            Assertion.AssertThat(hasItemsA, Not(HasItems(hasItemsC)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotPositive2()
        {
            Assertion.AssertThat(hasItemsA, Not(HasItems(new[] { "qwerty6" })));
        }


        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative1()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, HasItems(hasItemsD)); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNegative2()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, HasItems(new[] { "qwert12y" })); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullNegative3()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, HasItems(hasItemsB)); });
        }


        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative1()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, Not(HasItems(hasItemsB))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative2()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, Not(HasItems(new[] { "qwerty" }))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative3()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, Not(HasItems(hasItemsA))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsWithNotNegative4()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, Not(HasItems(hasItemsD))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemsNullWithNotNegative5()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, Not(HasItems(hasItemsB))); });
        }

        #endregion



        #region "HasItem"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemPositive1()
        {
            Assertion.AssertThat(hasItemsA, HasItem("qwerty"));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotPositive1()
        {
            Assertion.AssertThat(hasItemsA, Not(HasItem("qwerty27")));
        }


        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNegative1()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, HasItem("qwerty27")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemNullNegative2()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, HasItem("qwerty")); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNegative1()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(hasItemsA, Not(HasItem("qwerty"))); });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherHasItemWithNotNullNegative2()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, Not(HasItem("qwerty"))); });
        }


        #endregion



        #region "IsNullOrEmpty"

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive1()
        {
            Assertion.AssertThat(null, IsNullOrEmpty());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive2()
        {
            Assertion.AssertThat(new List<string>(), IsNullOrEmpty());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyPositive3()
        {
            Assertion.AssertThat(new string[0], IsNullOrEmpty());
        }


        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive1()
        {
            Assertion.AssertThat(hasItemsA, Not(IsNullOrEmpty()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestMatcherIsNullOrEmptyWithNotPositive2()
        {
            Assertion.AssertThat(new int[2] {2, 3}, Not(IsNullOrEmpty()));
        }



        #endregion
    }
}
