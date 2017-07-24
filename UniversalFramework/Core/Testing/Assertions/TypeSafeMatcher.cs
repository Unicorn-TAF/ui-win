using System;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class TypeSafeMatcher<T> : Matcher
    {

        protected virtual bool MatchesSafely(object _object)
        {
            try
            {
                ((T)_object).ToString();
                return true;
            }
            catch
            {
                Description.Append($"Unable to cast item to {typeof(T)}");
                return false;
            }
        }
    }
}
