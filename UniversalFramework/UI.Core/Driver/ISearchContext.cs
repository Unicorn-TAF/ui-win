using System.Collections.Generic;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Driver
{
    public interface ISearchContext
    {

        T Find<T>(ByLocator locator) where T : IControl;


        //T Find<T>(string name, string alternativeName) where T : IControl;


        IList<T> FindList<T>(ByLocator locator) where T : IControl;


        bool WaitFor<T>(ByLocator locator, int timeout) where T : IControl;


        bool WaitFor<T>(ByLocator locator, int timeout, out T controlInstance) where T : IControl;


        T FirstChild<T>() where T : IControl;
    }
}
