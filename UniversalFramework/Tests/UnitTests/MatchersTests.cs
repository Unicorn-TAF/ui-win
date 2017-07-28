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
        public void IsNullMatcherPositiveTest()
        {
            Assertion.AssertThat(null, IsNull());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsNullMatcherNegativeTest()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat("a", IsNull()); }
            );
        }

        #endregion


        #region IsEqualTo

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherPositiveStringTest()
        {
            Assertion.AssertThat("asd", IsEqualTo(("asd")));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherPositiveNumberTest()
        {
            Assertion.AssertThat(2, IsEqualTo((2)));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherPositiveCustomObjectTest()
        {
            Assertion.AssertThat(new SampleObject(), IsEqualTo(new SampleObject()));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherNegativeStringTest()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat("asd", IsEqualTo(("sd"))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherNegativeCustomObjectTest()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(new SampleObject(), IsEqualTo(new SampleObject("ds", 234))); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherNegativeNotCastableTest()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(new SampleObject(), IsEqualTo(23)); }
            );
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void IsEqualToMatcherNegativeNullTest()
        {
            Assert.Throws<AssertionError>(
                delegate { Assertion.AssertThat(null, IsEqualTo(23)); }
            );
        }

        #endregion



        [Test]
        public void MatcherTest()
        {
            List<string> a = new List<string>();
            a.Add("qwerty");
            a.Add("qwerty12");
            a.Add("qwerty123");

            List<string> b = new List<string>();
            b.Add("qwerty1");
            b.Add("qwerty123");

            Assertion.AssertThat(a, Not(HasItems(b)));
            //Assertion.AssertThat(a, Not(HasItem(2)));
            //Assertion.AssertThat(4, Not(IsEven()));
            //Assertion.AssertThat(24, Not(IsEqualTo(24)));
        }


        [Test, Author("Vitaliy Dobriyan")]
        public void HasItemsPositive1Test()
        {
            List<string> a = new List<string>();
            a.Add("qwerty");
            a.Add("qwerty12");
            a.Add("qwerty123");

            List<string> b = new List<string>();
            b.Add("qwerty");
            b.Add("qwerty123");

            Assertion.AssertThat(a, HasItems(b));
        }
    }
}
