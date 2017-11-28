using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Core.Controls.Interfaces.Typified
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
