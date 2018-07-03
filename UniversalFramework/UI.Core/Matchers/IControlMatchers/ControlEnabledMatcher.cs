using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlEnabledMatcher : Matcher
    {
        public ControlEnabledMatcher()
        {
        }

        public override string CheckDescription => "enabled";

        public override bool Matches(object obj)
        {
            return /*IsNotNull(obj) &&*/ Assertion(obj);
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

            bool enabled = element.Enabled;
            DescribeMismatch(enabled ? "enabled" : "disabled");

            return enabled;
        }
    }
}
