using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    public class DialPad : AndroidContainer
    {
        [Find(Using.Id, "com.android.dialer:id/one")]
        public AndroidControl ButtonOne { get; set; }

        [Find(Using.Id, "com.android.dialer:id/two")]
        public AndroidControl ButtonTwo { get; set; }

        [Find(Using.Id, "com.android.dialer:id/three")]
        public AndroidControl ButtonThree { get; set; }

        [Find(Using.Id, "com.android.dialer:id/four")]
        public AndroidControl ButtonFour { get; set; }

        [Find(Using.Id, "com.android.dialer:id/five")]
        public AndroidControl ButtonFive { get; set; }

        [Find(Using.Id, "com.android.dialer:id/six")]
        public AndroidControl ButtonSix { get; set; }

        [Find(Using.Id, "com.android.dialer:id/seven")]
        public AndroidControl ButtonSeven { get; set; }

        [Find(Using.Id, "com.android.dialer:id/eight")]
        public AndroidControl ButtonEight { get; set; }

        [Find(Using.Id, "com.android.dialer:id/nine")]
        public AndroidControl ButtonNine { get; set; }

        [Find(Using.Id, "com.android.dialer:id/star")]
        public AndroidControl ButtonStar { get; set; }

        [Find(Using.Id, "com.android.dialer:id/pound")]
        public AndroidControl ButtonPound { get; set; }

        [Find(Using.Id, "com.android.dialer:id/digits")]
        public AndroidControl InputNumber { get; set; }

        public AndroidControl GetButton(string name)
        {
            return this.Find<AndroidControl>(ByLocator.Id($"com.android.dialer:id/{name}"));
        }
    }
}
