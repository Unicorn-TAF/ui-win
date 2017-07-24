using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Testing.Assertions
{
    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override void DescribeTo()
        {
            Description.Append("An Even number");
        }

        public override bool Matches(object a)
        {
            if (!MatchesSafely(a))
                return false;

            Description.Append("was ").Append(a).Append(", which is an Odd number");

            return (int)a % 2 == 0;
        }

        public static IsEvenMatcher IsEven()
        {
            return new IsEvenMatcher();
        }
    }
}
