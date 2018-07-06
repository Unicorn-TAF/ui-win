using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.PageObject;

namespace ProjectSpecific.UI.Web
{
    public class PageYandexMarketMain : WebPage
    {
        [Name("Top Navigation Menu")]
        [Find(Using.Web_Css, ".topmenu__list")]
        public YandexTopMenu MenuTop;

        [Name("Mobile Phones link")]
        [Find(Using.Web_Xpath, "//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']")]
        public WebControl LinkMobilePhones;

        public PageYandexMarketMain() : base("", "Яндекс.Маркет — выбор и покупка товаров из проверенных интернет-магазинов") { }
    }
}
