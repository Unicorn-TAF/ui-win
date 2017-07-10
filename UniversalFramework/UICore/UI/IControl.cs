using System;
using Unicorn.UICore.Driver;
using Unicorn.UICore.UIProperties;

namespace Unicorn.UICore.UI
{
    public interface IControl// : ISearchContext
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

        void WaitForEnabled();

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

        UIPoint Location {
            get;
        }

        UISize Size {
            get;
        }

        #endregion
    }
}
