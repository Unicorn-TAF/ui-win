using ProjectSpecific.UI.Web;
using Unicorn.UI.Web.PageObject;

namespace ProjectSpecific.UI
{
    public class YandexMarketSite : WebSite
    {
        public YandexMarketSite(string baseUrl) : base(baseUrl) { }

        public PageYandexMarketMain MainPage => this.GetPage<PageYandexMarketMain>();
    }
}
