using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia
{
    public class CelestiaSite : WebSite
    {
        private static CelestiaSite _instance = null;

        public CelestiaSite() : base("https://celestia.space")
        {
        }

        public static CelestiaSite Instance => 
            _instance ?? (_instance = new CelestiaSite());
    }
}
