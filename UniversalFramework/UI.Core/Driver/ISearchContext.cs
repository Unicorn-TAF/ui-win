using System.Collections.Generic;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Driver
{
    public interface ISearchContext
    {
        T Find<T>(ByLocator locator) where T : IControl;

        IList<T> FindList<T>(ByLocator locator) where T : IControl;

        bool WaitFor<T>(ByLocator locator, int millisecondsTimeout) where T : IControl;

        bool WaitFor<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl;
    }
}
