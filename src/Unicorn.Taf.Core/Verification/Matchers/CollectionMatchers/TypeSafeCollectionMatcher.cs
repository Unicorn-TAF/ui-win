using System.Collections.Generic;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    public abstract class TypeSafeCollectionMatcher<T> : AbstractMatcher
    {
        protected TypeSafeCollectionMatcher() : base()
        {
        }

        public abstract bool Matches(IEnumerable<T> actual);
    }
}
