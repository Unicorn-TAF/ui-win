using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class ContainsMatcher : Matcher
    {
        private object ObjectToCompare;


        public ContainsMatcher(object objectToCompare)
        {
            ObjectToCompare = objectToCompare;
        }



        public override void DescribeTo()
        {
            Description.Append("Contains " + ObjectToCompare);
        }


        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && Assertion(_object);
        }


        protected bool Assertion(object _object)
        {
            if (!ObjectToCompare.GetType().Equals(_object.GetType()))
            {
                Description.Append($"was not of type {ObjectToCompare.GetType()}");
                return false;
            }

            string _objString = (string)_object;
            string _objToCompareString = (string)ObjectToCompare;

            bool contains = _objString.Contains(_objToCompareString);
            if (!contains)
                DescribeMismatch(_object);

            return contains;
        }
    }


    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override void DescribeTo()
        {
            Description.Append("An Even number");
        }

        protected override bool Assertion(object number)
        {
            bool isEven = (int)number % 2 == 0;
            if (!isEven)
                DescribeMismatch(number);

            return isEven;
        }

        public override void DescribeMismatch(object number)
        {
            base.DescribeMismatch(number);
            //Description.Append(", which is an Odd number");
        }
    }

}
