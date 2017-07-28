using System.Text;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class Matcher
    {
        public StringBuilder Description;

        protected bool NullCheckable = true;


        public Matcher()
        {
            Description = new StringBuilder();
        }



        public abstract void DescribeTo();


        public virtual void DescribeMismatch(object _object)
        {
            Description.Append("was ").Append(_object);
        }


        public abstract bool Matches(object _object);


        protected bool IsNotNull(object _object)
        {
            if (NullCheckable && _object == null)
            {
                Description.Append("was null");
                return false;
            }
            return true;
        }


        public override string ToString()
        {
            return Description.ToString();
        }
    }
}
