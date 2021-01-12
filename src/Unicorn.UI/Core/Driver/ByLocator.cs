namespace Unicorn.UI.Core.Driver
{
    /// <summary>
    /// Represents UI controls search method.
    /// </summary>
    public enum Using
    {
        /// <summary>
        /// Search by Css (web only)
        /// </summary>
        WebCss,

        /// <summary>
        /// Search by Xpath (web only)
        /// </summary>
        WebXpath,

        /// <summary>
        /// Search by tag property (web only)
        /// </summary>
        WebTag,

        /// <summary>
        /// Search by class name
        /// </summary>
        Class,

        /// <summary>
        /// Search by control name
        /// </summary>
        Name,

        /// <summary>
        /// Search by control Id
        /// </summary>
        Id
    }

    /// <summary>
    /// Represents control locator which consists of search method and search query.
    /// </summary>
    public class ByLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByLocator"/> class with specified search method and search query.
        /// </summary>
        /// <param name="how">search method</param>
        /// <param name="locator">locator to search by</param>
        public ByLocator(Using how, string locator)
        {
            How = how;
            Locator = locator;
        }

        /// <summary>
        /// Gets or sets search method.
        /// </summary>
        public Using How { get; protected set; }

        /// <summary>
        /// Gets or sets search query.
        /// </summary>
        public string Locator { get; protected set; }

        /// <summary>
        /// Gets Id locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Id(string locator) => new ByLocator(Using.Id, locator);

        /// <summary>
        /// Gets Name locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Name(string locator) => new ByLocator(Using.Name, locator);

        /// <summary>
        /// Gets Class name locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Class(string locator) => new ByLocator(Using.Class, locator);

        /// <summary>
        /// Gets Css locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Css(string locator) => new ByLocator(Using.WebCss, locator);

        /// <summary>
        /// Gets Tag locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Tag(string locator) => new ByLocator(Using.WebTag, locator);

        /// <summary>
        /// Gets Xpath locator.
        /// </summary>
        /// <param name="locator">search query</param>
        /// <returns>xpath locator</returns>
        public static ByLocator Xpath(string locator) => new ByLocator(Using.WebXpath, locator);

        /// <summary>
        /// Gets readable string.
        /// </summary>
        /// <returns>description</returns>
        public override string ToString() => $"{How} = {Locator}";
    }
}
