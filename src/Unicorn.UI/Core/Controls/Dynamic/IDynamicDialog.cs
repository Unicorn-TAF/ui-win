using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public enum DialogElement
    {
        Title = 1,
        Content = 2,
        Accept = 3,
        Decline = 4,
        Close = 5,
        Loader = 6
    }

    public interface IDynamicDialog : IDynamicControl, IModalWindow, ILoadable
    {
        IControl TitleControl { get; }

        IControl ContentControl { get; }

        IControl AcceptButton { get; }

        IControl DeclineButton { get; }

        IControl CloseButton { get; }
    }
}
