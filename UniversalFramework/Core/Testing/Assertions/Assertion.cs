using System;
using System.Text;

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



    public class SoftAssertion
    {
        StringBuilder Errors;
        bool IsSomethingFailed;
        int ErrorCounter;

        public SoftAssertion()
        {
            Errors = new StringBuilder();
            IsSomethingFailed = false;
            ErrorCounter = 1;
        }

        public SoftAssertion AssertThat(object _object, Matcher matcher)
        {
            AssertThat("", _object, matcher);
            return this;
        }


        public SoftAssertion AssertThat(string message, object _object, Matcher matcher)
        {
            matcher.Description.Append("Expected ");
            matcher.DescribeTo();
            matcher.Description.AppendLine("").Append("But ");

            

            if (!matcher.Matches(_object))
            {
                if (!string.IsNullOrEmpty(message))
                    message += "\n";

                Errors.AppendLine($"Error {ErrorCounter++}").Append(message).Append(matcher.ToString()).Append("\n\n");
                IsSomethingFailed = true;
                
            }
            return this;
        }

        public void AssertAll()
        {
            if (IsSomethingFailed)
                throw new AssertionError("\n" + Errors.ToString().Trim());
        }
    }



    public class AssertionError : Exception
    {
        public AssertionError() : base() { }

        public AssertionError(string message) : base(message) { }
    }
}
