using UICore.UI;

namespace UICore.Driver
{
    public interface ISearchContext
    {
        T GetElement<T>(string locator) where T : IControl;

        T WaitForElement<T>(string locator) where T : IControl;
    }
}
