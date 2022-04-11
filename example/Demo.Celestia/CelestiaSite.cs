using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia
{
    /// <summary>
    /// Describes https://celestia.space website (should inherit <see cref="WebSite"/>).
    /// </summary>
    public class CelestiaSite : WebSite
    {
        private static CelestiaSite _instance = null;

        /// <summary>
        /// Website constructor. Calls base constructor with address to website.
        /// </summary>
        public CelestiaSite() : base("https://celestia.space")
        {
        }

        public static CelestiaSite Instance => 
            _instance ?? (_instance = new CelestiaSite());
    }
}
