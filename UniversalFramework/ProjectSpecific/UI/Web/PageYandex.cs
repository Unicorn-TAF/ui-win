using System;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.PageObject;

namespace ProjectSpecific.UI.Web
{
    public class PageYandex : WebPage
    {
        [Find(Using.Web_Css, ".topmenu__list")]
        public YandexTopMenu MenuTop;

        [Find(Using.Web_Xpath, "//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']")]
        public WebControl LinkMobilePhones;

        public override void WaitForPageLoading()
        {
            throw new NotImplementedException();
        }
    }
}
