using Unicorn.UIWeb;
using Unicorn.UIWeb.Driver;

namespace ProjectSpecific
{
    public class Web
    {

        private static WebDriver _instance = null;
        private static Browser browser = Browser.CHROME;

        public static WebDriver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WebDriver(browser);

                return _instance;
            }
        }

        public static void Quit()
        {
            if (_instance != null)
            {
                Instance.Close();
                _instance = null;
            }
        }

    }
}
