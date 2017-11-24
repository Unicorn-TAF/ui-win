using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Core.UI.Controls
{
    public interface IWindow : IContainer
    {
        string Title
        {
            get;
        }

        void Close();

        void WaitForClosed(int timeout);
    }
}
