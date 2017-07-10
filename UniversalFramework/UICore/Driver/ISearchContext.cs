using System.Collections.Generic;
using Unicorn.UICore.UI;

namespace Unicorn.UICore.Driver
{
    public interface ISearchContext
    {
        T FindControl<T>(By by, string locator) where T : IControl;

        IList<T> FindControls<T>(By by, string locator) where T : IControl;
    }
}
