using System.Drawing;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    public interface IControl
    {
        /// <summary>
        /// Element locator
        /// </summary>
        ByLocator Locator { get; set; }


        #region "Props"

        string GetAttribute(string attribute);

        bool Visible {
            get;
        }

        bool Enabled {
            get;
        }

        string Text {
            get;
        }

        Point Location {
            get;
        }

        Rectangle BoundingRectangle {
            get;
        }

        #endregion


        #region "Methods"

        void Click();

        void RightClick();

        #endregion

    }
}
