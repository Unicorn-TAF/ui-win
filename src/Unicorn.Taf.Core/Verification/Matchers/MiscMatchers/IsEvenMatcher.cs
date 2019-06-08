﻿namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override string CheckDescription => "An Even number";

        public override bool Matches(int actual)
        {
            DescribeMismatch(actual.ToString());
            return actual % 2 == 0;
        }
    }
}
