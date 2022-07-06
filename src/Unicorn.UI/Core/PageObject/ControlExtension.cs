using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    public static class ControlExtension
    {
        public static bool ExistsInPageObject<T>(this T control) where T : IControl
        {
            ISearchContext context = control.GetType()
                .GetProperty(InternalResources.ParentContext).GetValue(control) as ISearchContext;

            return context.TryGetChild<T>(control.Locator);
        }
    }
}
