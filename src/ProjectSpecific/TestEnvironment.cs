using ProjectSpecific.UI;

namespace ProjectSpecific
{
    public class TestEnvironment
    {
        private static TestEnvironment instance = null;

        protected TestEnvironment()
        {
            this.YandexMarket = new YandexMarketSite(@"https://market.yandex.ru/");
            this.Charmap = new CharmapApplication(@"C:\Windows\System32\", "charmap.exe");
        }

        public static TestEnvironment Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestEnvironment();
                }

                return instance;
            }
        }

        public YandexMarketSite YandexMarket { get; protected set; }

        public CharmapApplication Charmap { get; protected set; }
    }
}
