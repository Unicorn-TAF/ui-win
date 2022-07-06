using System.Collections.Generic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Mobile.Android.Controls;

namespace Unicorn.UnitTests.UI.Gui.Android
{
    public class DialPad : AndroidControl
    {
        [ById("com.google.android.dialer:id/digits")]
        public AndroidControl InputNumber { get; set; }

        public IList<AndroidControl> Buttons => FindList<AndroidControl>(ByLocator.Xpath(
            "//android.widget.FrameLayout[@content-desc]"));

        public AndroidControl GetButton(string name) =>
            Find<AndroidControl>(ByLocator.Id($"com.google.android.dialer:id/{GetNumberAsText(name)}"));

        private string GetNumberAsText(string name)
        {
            if (name == "*")
            {
                return "star";
            }

            if (name == "#")
            {
                return "pound";
            }

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            return unitsMap[int.Parse(name)];
        }
    }
}
