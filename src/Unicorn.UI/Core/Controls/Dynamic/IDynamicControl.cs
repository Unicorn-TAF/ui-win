using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// Interface for dynamically defined UI controls.
    /// </summary>
    public interface IDynamicControl
    {
        /// <summary>
        /// Populates UI control with dictionary of sub-elements and their locators.
        /// </summary>
        /// <param name="elementsLocators">dictionary (key: integer sub-element index; value - sub-element locator)</param>
        void Populate(Dictionary<int, ByLocator> elementsLocators);
    }
}
