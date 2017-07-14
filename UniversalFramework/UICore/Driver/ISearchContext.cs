using System.Collections.Generic;
using Unicorn.UICore.UI;

namespace Unicorn.UICore.Driver
{
    public interface ISearchContext
    {

        T Find<T>(By by, string locator) where T : IControl;


        IList<T> FindList<T>(By by, string locator) where T : IControl;


        bool WaitFor<T>(By by, string locator, int timeout) where T : IControl;


        bool WaitFor<T>(By by, string locator, int timeout, out T controlInstance) where T : IControl;


        T FirstChild<T>() where T : IControl;
    }
}
