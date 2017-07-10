using System.Collections.Generic;
using Unicorn.UICore.UI;

namespace Unicorn.UICore.Driver
{
    public interface ISearchContext
    {
        T GetElement<T>(string locator) where T : IControl;

        T WaitForElement<T>(string locator) where T : IControl;

        IList<T> FindControls<T>(string locator) where T : IControl;
    }
}
