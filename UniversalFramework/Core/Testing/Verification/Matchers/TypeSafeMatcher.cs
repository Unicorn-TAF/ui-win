namespace Unicorn.Core.Testing.Verification.Matchers
{
    public abstract class TypeSafeMatcher<T> : Matcher
    {
        public override bool Matches(object obj)
        {
            return this.IsNotNull(obj) && this.CouldBeCasted(obj) && this.Assertion(obj);
        }

        protected bool CouldBeCasted(object obj)
        {
            bool couldBeCasted = obj is T;

            if (!couldBeCasted)
            {
                DescribeMismatch($"not of type {typeof(T)}");
            }

            return couldBeCasted;
        }

        protected abstract bool Assertion(object obj);
    }
}
