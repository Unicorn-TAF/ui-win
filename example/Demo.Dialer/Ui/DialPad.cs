using System.Collections.Generic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    [Name("Dialpad")]
    [Find(Using.Id, "com.google.android.dialer:id/dialtacts_mainlayout")]
    public class DialPad : AndroidContainer
    {
        private readonly string[] _unitsMap = 
            new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        [ById("com.google.android.dialer:id/digits")]
        public AndroidControl NumberText { get; set; }

        [Find(Using.WebXpath, "//android.widget.FrameLayout[@content-desc]")]
        public IList<AndroidControl> Buttons { get; set; }

        public AndroidControl GetButton(string name) =>
            Find<AndroidControl>(ByLocator.Id($"com.google.android.dialer:id/{GetButtonId(name)}"));

        private string GetButtonId(string name)
        {
            if (name == "*")
            {
                return "star";
            }

            if (name == "#")
            {
                return "pound";
            }

            return _unitsMap[int.Parse(name)];
        }
    }
}
