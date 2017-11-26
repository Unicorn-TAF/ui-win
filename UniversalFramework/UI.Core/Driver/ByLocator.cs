
namespace Unicorn.UI.Core.Driver
{
    public class ByLocator
    {
        public readonly Using How;
        public readonly string Locator;

        public ByLocator(Using how, string locator)
        {
            this.How = how;
            this.Locator = locator;
        }


        public static ByLocator Id(string locator)
        {
            return new ByLocator(Using.Id, locator);
        }


        public static ByLocator Name(string locator)
        {
            return new ByLocator(Using.Name, locator);
        }


        public static ByLocator Class(string locator)
        {
            return new ByLocator(Using.Class, locator);
        }

        public static ByLocator Css(string locator)
        {
            return new ByLocator(Using.Web_Css, locator);
        }

        public static ByLocator Tag(string locator)
        {
            return new ByLocator(Using.Web_Tag, locator);
        }

        public static ByLocator Xpath(string locator)
        {
            return new ByLocator(Using.Web_Xpath, locator);
        }

        public override string ToString()
        {
            return $"{How} = {Locator}";
        }
    }

    public enum Using
    {
        Web_Css,
        Web_Xpath,
        Web_Tag,
        Class,
        Name,
        Id
    }
}
