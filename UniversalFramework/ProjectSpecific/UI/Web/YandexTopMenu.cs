using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace ProjectSpecific.UI.Web
{
    public class YandexTopMenu : WebContainer
    {
        [Name("Link Electronics")]
        [Find(Using.Web_Css, "[data-department = Электроника]")]
        public WebControl LinkElectronics;
    }
}
