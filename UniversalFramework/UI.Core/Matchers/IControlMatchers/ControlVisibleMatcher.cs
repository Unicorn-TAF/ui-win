using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlVisibleMatcher : Matcher
    {
        public ControlVisibleMatcher()
        {
        }

        public override string CheckDescription => "visible";

        public override bool Matches(object obj)
        {
            return IsNotNull(obj) && Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            IControl element = null;

            try
            {
                element = obj as IControl;
            }
            catch
            {
                DescribeMismatch("was not an instance of IControl");
                return !this.Reverse;
            }

            bool visible = element.Visible;
            DescribeMismatch(visible ? "visible" : "not visible");

            return visible;
        }
    }
}
