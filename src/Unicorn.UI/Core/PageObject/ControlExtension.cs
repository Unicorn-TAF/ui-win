using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Extensions for <see cref="IControl"/>
    /// </summary>
    public static class ControlExtension
    {
        /// <summary>
        /// Checks if control exists in page object immediately ignoring implicitly wait.<br/>
        /// Works only for controls located by <see cref="FindAttribute"/>.
        /// </summary>
        /// <typeparam name="T">control type (should implement <see cref="IControl"/>)</typeparam>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool ExistsInPageObject<T>(this T control) where T : IControl
        {
            ISearchContext context = control.GetType()
                .GetProperty(InternalResources.ParentContext).GetValue(control) as ISearchContext;

            return context.TryGetChild<T>(control.Locator);
        }
    }
}
