using System;
using System.Drawing;
using Unicorn.UICore.Driver;

namespace Unicorn.UICore.UI
{
    public interface IControl
    {

        #region "Methods"

        void Click();

        #endregion


        #region "Assertions"

        void CheckAttributeContains(string attribute, string expectedValue);

        void CheckAttributeDoeNotContain(string attribute, string expectedValue);

        void CheckAttributeEquals(string attribute, string expectedValue);

        #endregion


        #region "Waiters"

        void WaitForEnabled(int timeout);

        void WaitForAttributeValue(string attribute, string value, bool contains = true);

        #endregion


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

        Size Size {
            get;
        }

        #endregion
    }
}
