using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.PageObject;

namespace ProjectSpecific.UI.Web
{
    public class PageYandexMarketMain : WebPage
    {
        private readonly string title = "Яндекс.Маркет — выбор и покупка товаров из проверенных интернет-магазинов";
        private readonly string url = "";

        [Find(Using.Web_Css, ".topmenu__list")]
        public YandexTopMenu MenuTop;

        [Find(Using.Web_Xpath, "//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']")]
        public WebControl LinkMobilePhones;

        public override string Url => url;

        public override string Title => title;
    }
}
