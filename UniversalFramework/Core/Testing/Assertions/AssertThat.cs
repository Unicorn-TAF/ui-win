using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Unicorn.Core.Testing.Assertions
{
    public class Assert
    {
        public static void AssertThat(object item, Matcher matcher)
        {
            matcher.Description.Append("Check that: ");
            matcher.DescribeTo();
            matcher.Description.Append(". ");

            if (!matcher.Matches(item))
                throw new AssertionError(matcher.Description.ToString());
        }
    }

    public class AssertionError : Exception
    {
        public AssertionError() : base() { }

        public AssertionError(string message) : base(message) { }
    }
}
