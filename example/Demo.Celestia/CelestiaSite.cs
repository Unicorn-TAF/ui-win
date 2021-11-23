using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia
{
    /// <summary>
    /// Describes website (celestia website).
    /// should inherit <see cref="Application"/>.
    /// </summary>
    public class CelestiaSite : WebSite
    {
        private static CelestiaSite _instance = null;

        /// <summary>
        /// Website constructor. Calls base constructor with site address.
        /// </summary>
        public CelestiaSite() : base("https://celestia.space")
        {
        }

        public static CelestiaSite Instance => 
            _instance ?? (_instance = new CelestiaSite());
    }
}
