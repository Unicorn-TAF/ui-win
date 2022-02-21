namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    /// <summary>
    /// Interface for modal windows implementation. 
    /// Has definitions of of basic methods and properties.
    /// Implements <see cref="IWindow"/>.
    /// </summary>
    public interface IModalWindow : IWindow
    {
        /// <summary>
        /// Gets modal window text content (foe example dialog message)
        /// </summary>
        string TextContent
        {
            get;
        }

        /// <summary>
        /// Accept modal window
        /// </summary>
        void Accept();

        /// <summary>
        /// Decline modal window
        /// </summary>
        void Decline();
    }
}
