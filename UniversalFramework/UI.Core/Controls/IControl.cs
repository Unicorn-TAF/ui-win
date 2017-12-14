using System.Drawing;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    public interface IControl
    {
        /// <summary>
        /// Gets or sets element locator
        /// </summary>
        ByLocator Locator { get; set; }

        #region "Props"

        bool Visible
        {
            get;
        }

        bool Enabled
        {
            get;
        }

        string Text
        {
            get;
        }

        Point Location
        {
            get;
        }

        Rectangle BoundingRectangle
        {
            get;
        }

        #endregion

        #region "Methods"

        string GetAttribute(string attribute);

        void Click();

        void RightClick();

        #endregion
    }
}
