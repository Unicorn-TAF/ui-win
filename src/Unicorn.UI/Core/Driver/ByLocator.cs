namespace Unicorn.UI.Core.Driver
{
    public enum Using
    {
        Web_Css,
        Web_Xpath,
        Web_Tag,
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

        public static ByLocator Css(string locator) => new ByLocator(Using.Web_Css, locator);

        public static ByLocator Tag(string locator) => new ByLocator(Using.Web_Tag, locator);

        public static ByLocator Xpath(string locator) => new ByLocator(Using.Web_Xpath, locator);

        public override string ToString() => $"{How} = {Locator}";
    }
}
