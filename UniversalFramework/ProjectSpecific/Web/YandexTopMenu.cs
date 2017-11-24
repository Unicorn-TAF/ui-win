using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace ProjectSpecific.Web
{
    public class YandexTopMenu : WebContainer
    {
        [Find(LocatorType.Web_Xpath, ".//li[@data-department='Электроника']/a")]
        public WebControl Link;
    }
}
