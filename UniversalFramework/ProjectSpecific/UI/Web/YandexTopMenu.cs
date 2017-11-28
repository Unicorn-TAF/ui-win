using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace ProjectSpecific.UI.Web
{
    public class YandexTopMenu : WebContainer
    {
        [Find(Using.Web_Xpath, ".//li[@data-department='Электроника']/a")]
        public WebControl Link;
    }
}
