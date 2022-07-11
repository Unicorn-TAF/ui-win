using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Mobile.Android.Controls;

namespace Demo.AndroidDialer.Ui
{
    [Name("Calls History")]
    [Find(Using.Id, "com.google.android.dialer:id/lists_pager")]
    public class CallsHistoryPane : AndroidControl
    {
        [ById("com.google.android.dialer:id/emptyListViewImage")]
        public AndroidControl EmptyListIcon { get; set; }
    }
}
