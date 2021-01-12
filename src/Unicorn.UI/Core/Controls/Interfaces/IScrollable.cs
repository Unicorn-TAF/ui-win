namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for controls which have scroll functionality.
    /// </summary>
    public interface IScrollable
    {
        /// <summary>
        /// Gets current horizontal scroll amount percentage.
        /// </summary>
        int CurrentHorizontalScrollAmount { get; }

        /// <summary>
        /// Gets current vertical scroll amount percentage.
        /// </summary>
        int CurrentVerticalScrollAmount { get; }

        /// <summary>
        /// Scroll vertically by specified percent.
        /// </summary>
        /// <param name="scrollPercentage">percentage of scrolling (from -100% to 100%)</param>
        /// <returns>true - if scrolled; false - if unable to scroll in specified direction</returns>
        bool ScrollVertical(int scrollPercentage);

        /// <summary>
        /// Scroll horizontal by specified percent.
        /// </summary>
        /// <param name="scrollPercentage">percentage of scrolling (from -100% to 100%)</param>
        /// <returns>true - if scrolled; false - if unable to scroll in specified direction</returns>
        bool ScrollHorisontal(int scrollPercentage);
    }
}
