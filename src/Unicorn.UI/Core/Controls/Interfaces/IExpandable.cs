namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for controls which have expand/collapse functionality.
    /// </summary>
    public interface IExpandable
    {
        /// <summary>
        /// Gets a value indicating whether control is expanded.
        /// </summary>
        bool Expanded
        {
            get;
        }

        /// <summary>
        /// Perform expanding.
        /// </summary>
        /// <returns>true - if expand was performed; false - if already expanded</returns>
        bool Expand();

        /// <summary>
        /// Perform collapsing.
        /// </summary>
        /// <returns>true - if collapse was performed; false - if already collapsed</returns>
        bool Collapse();
    }
}
