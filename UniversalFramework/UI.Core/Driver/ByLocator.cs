
namespace Unicorn.UI.Core.Driver
{
    public class ByLocator
    {
        public readonly LocatorType How;
        public readonly string Locator;

        public ByLocator(LocatorType how, string locator)
        {
            this.How = how;
            this.Locator = locator;
        }


        public static ByLocator Id(string locator)
        {
            return new ByLocator(LocatorType.Id, locator);
        }


        public static ByLocator Name(string locator)
        {
            return new ByLocator(LocatorType.Name, locator);
        }


        public static ByLocator Class(string locator)
        {
            return new ByLocator(LocatorType.Class, locator);
        }

        public static ByLocator Css(string locator)
        {
            return new ByLocator(LocatorType.Web_Css, locator);
        }

        public static ByLocator Tag(string locator)
        {
            return new ByLocator(LocatorType.Web_Tag, locator);
        }

        public static ByLocator Xpath(string locator)
        {
            return new ByLocator(LocatorType.Web_Xpath, locator);
        }

        public override string ToString()
        {
            return $"{How} = {Locator}";
        }
    }

    public enum LocatorType
    {
        Web_Css,
        Web_Xpath,
        Web_Tag,
        Class,
        Name,
        Id
    }
}
