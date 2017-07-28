using System;

namespace Unicorn.Core.Testing.Assertions
{
    public class Assertion
    {

        public static void AssertThat(object _object, Matcher matcher)
        {
            AssertThat("", _object, matcher);
        }


        public static void AssertThat(string message, object _object, Matcher matcher)
        {
            matcher.Description.Append("Expected ");
            matcher.DescribeTo();
            matcher.Description.AppendLine("").Append("But ");

            if (!matcher.Matches(_object))
            {
                throw new AssertionError(message + "\n" + matcher.ToString());
            }
        }
    }



    public class AssertionError : Exception
    {
        public AssertionError() : base() { }

        public AssertionError(string message) : base(message) { }
    }
}
