using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// UI dialog sub-elements.
    /// </summary>
    public enum DialogElement
    {
        /// <summary>
        /// Dialog title.
        /// </summary>
        Title = 1,

        /// <summary>
        /// Dialog content.
        /// </summary>
        Content = 2,

        /// <summary>
        /// Dialog acceptor.
        /// </summary>
        Accept = 3,

        /// <summary>
        /// Dialog decliner.
        /// </summary>
        Decline = 4,

        /// <summary>
        /// Dialog close control.
        /// </summary>
        Close = 5,

        /// <summary>
        /// Dialog content load indicator.
        /// </summary>
        Loader = 6
    }

    /// <summary>
    /// Interface for dynamically defined UI dialog.
    /// </summary>
    public interface IDynamicDialog : IDynamicControl, IModalWindow, ILoadable
    {
        /// <summary>
        /// Gets title control.
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        IControl GetTitleControl();

        /// <summary>
        /// Gets dialog content control.
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        IControl GetContentControl();

        /// <summary>
        /// Gets control of button for dialog acceptance.
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        IControl GetAcceptButton();

        /// <summary>
        /// Gets control of button for dialog declining.
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        IControl GetDeclineButton();

        /// <summary>
        /// Gets control of dialog close button.
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        IControl GetCloseButton();
    }
}
