namespace Unicorn.UI.Core.Driver
{
    public enum Using
    {
        WebCss,
        WebXpath,
        WebTag,
        Class,
        Name,
        Id
    }

    public class ByLocator
    {
        public ByLocator(Using how, string locator)
        {
            this.How = how;
            this.Locator = locator;
        }

        public Using How { get; protected set; }

        public string Locator { get; protected set; }

        public static ByLocator Id(string locator) => new ByLocator(Using.Id, locator);

        public static ByLocator Name(string locator) => new ByLocator(Using.Name, locator);

        public static ByLocator Class(string locator) => new ByLocator(Using.Class, locator);

        public static ByLocator Css(string locator) => new ByLocator(Using.WebCss, locator);

        public static ByLocator Tag(string locator) => new ByLocator(Using.WebTag, locator);

        public static ByLocator Xpath(string locator) => new ByLocator(Using.WebXpath, locator);

        public override string ToString() => $"{How} = {Locator}";
    }
}
