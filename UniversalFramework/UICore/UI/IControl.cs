using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Unicorn.UICore.Driver;

namespace Unicorn.UICore.UI
{
    public interface IControl
    {

        #region "Methods"

        void Click();

        void RightClick();

        #endregion


        #region "Assertions"

        //void CheckAttributeContains(string attribute, string expectedValue);

        //void CheckAttributeDoeNotContain(string attribute, string expectedValue);

        //void CheckAttributeEquals(string attribute, string expectedValue);

        #endregion


        #region "Waiters"

        //void WaitForEnabled(int timeout);

        //void WaitForAttributeValue(string attribute, string value, bool contains, int timeout);

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

    public static class IControlExtension
    {

        public static void CheckAttributeContains(this IControl control, string attribute, string expectedValue)
        {
            string actual = control.GetAttribute(attribute);
            Assert.IsTrue(actual.Contains(expectedValue), $"Control property '{attribute}' does not contain expected value '{expectedValue}'");
        }


        public static void CheckAttributeDoesNotContain(this IControl control, string attribute, string expectedValue)
        {
            string actual = control.GetAttribute(attribute);
            Assert.IsFalse(actual.Contains(expectedValue), $"Control property '{attribute}' does contains unexpected value '{expectedValue}'");
        }


        public static void CheckAttributeEquals(this IControl control, string attribute, string expectedValue)
        {
            string actual = control.GetAttribute(attribute);
            Assert.AreEqual(expectedValue, actual, $"Control property '{attribute}' does not equal to expected value");
        }


        public static void WaitForEnabled(this IControl control, int timeout = 10000)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while ((!control.Enabled || !control.Visible) && timer.ElapsedMilliseconds < timeout)
            {
                Thread.Sleep(50);
            }
            if (timer.ElapsedMilliseconds >= timeout)
                throw new ControlInvalidStateException($"Control is in illegal state. Visible: {control.Visible}, Enabled: {control.Enabled}");
        }


        public static void WaitForAttributeValue(this IControl control, string attribute, string value, bool contains = true, int timeout = 10000)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            bool condition = true;
            do
            {
                Thread.Sleep(50);
                string actualValue = control.GetAttribute(attribute);
                if (contains)
                    condition = actualValue.Contains(value);
                else
                    condition = actualValue.Equals(value);
            } while (condition && timer.ElapsedMilliseconds < timeout);

            if (timer.ElapsedMilliseconds >= timeout)
                throw new ControlInvalidStateException($"Control property {attribute} was not changed to: {value}");
        }
    }
}
