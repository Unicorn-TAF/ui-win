using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Core.Testing.Assertions.Matchers;

namespace Unicorn.Core.Testing.Assertions
{
    public class MiscMatchers
    {
        public static IsEvenMatcher IsEven()
        {
            return new IsEvenMatcher();
        }

        public static ContainsMatcher Contains(string objectToCompare)
        {
            return new ContainsMatcher(objectToCompare);
        }
    }
}
