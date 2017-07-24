using System;
using System.Text;

namespace Unicorn.Core.Testing.Assertions
{
    public abstract class Matcher
    {
        public StringBuilder Description;

        public Matcher()
        {
            Description = new StringBuilder();
        }

        public virtual void DescribeTo()
        {
            throw new NotImplementedException();
        }


        public virtual bool Matches(object _object)
        {
            throw new NotImplementedException();
        }

    }
}
