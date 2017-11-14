using System;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class TypeSafeMatcher<T> : Matcher
    {

        protected bool IsCastable(object _object)
        {
            bool isCastable = _object is T;
            
            if(!isCastable)
                MatcherOutput.Append($"was not of type {typeof(T)}");

            return isCastable;
        }

        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && IsCastable(_object) && Assertion(_object);
        }

        protected abstract bool Assertion(object _object);
    }
}
