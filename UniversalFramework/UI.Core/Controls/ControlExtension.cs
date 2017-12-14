using System.Diagnostics;
using System.Threading;

namespace Unicorn.UI.Core.Controls
{
    public static class ControlExtension
    {
        public static void WaitForEnabled(this IControl control, int timeout = 10000)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            bool disabled = true;
            do
            {
                Thread.Sleep(50);
                disabled = !control.Enabled || !control.Visible;
            }
            while (disabled && timer.ElapsedMilliseconds < timeout);

            if (timer.ElapsedMilliseconds >= timeout)
            {
                throw new ControlInvalidStateException($"Control is in illegal state. Visible: '{control.Visible}', Enabled: '{control.Enabled}'");
            }
        }

        public static void WaitForAttributeValue(this IControl control, string attribute, string value, bool exactMatch = false, int timeout = 10000)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            bool notMatches = true;
            do
            {
                Thread.Sleep(50);
                string actualValue = control.GetAttribute(attribute);

                if (exactMatch)
                {
                    notMatches = actualValue.Equals(value);
                }
                else
                {
                    notMatches = actualValue.Contains(value);
                }
            }
            while (notMatches && timer.ElapsedMilliseconds < timeout);

            if (timer.ElapsedMilliseconds >= timeout)
            {
                throw new ControlInvalidStateException($"Control '{attribute}' property was not changed to: '{value}'");
            }
        }
    }
}
