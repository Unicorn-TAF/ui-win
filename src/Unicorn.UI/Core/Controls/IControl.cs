using System.Drawing;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    /// <summary>
    /// Interface for UI control.
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether control is cached (cached control is searched for each time when it's called).
        /// </summary>
        bool Cached { get; set; }

        /// <summary>
        /// Gets or sets element locator.
        /// </summary>
        ByLocator Locator { get; set; }

        /// <summary>
        /// Gets or sets control readable name.
        /// </summary>
        string Name { get; set; }

        #region "Props"

        /// <summary>
        /// Gets a value indicating whether control is visible on screen.
        /// </summary>
        bool Visible
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether control is enabled.
        /// </summary>
        bool Enabled
        {
            get;
        }

        /// <summary>
        /// Gets control text.
        /// </summary>
        string Text
        {
            get;
        }

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        Point Location
        {
            get;
        }

        /// <summary>
        /// Gets control bounding rectangle as <see cref="Rectangle"/>
        /// </summary>
        Rectangle BoundingRectangle
        {
            get;
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Get string value of specified control attribute.
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <returns>attribute value</returns>
        string GetAttribute(string attribute);

        /// <summary>
        /// Perform click on control.
        /// </summary>
        void Click();

        /// <summary>
        /// Perform right click on control.
        /// </summary>
        void RightClick();

        #endregion
    }
}
