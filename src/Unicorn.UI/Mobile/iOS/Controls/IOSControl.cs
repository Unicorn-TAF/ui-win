using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Ios.Driver;

namespace Unicorn.UI.Mobile.Ios.Controls
{
    public class IosControl : IosSearchContext, IControl
    {
        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public AppiumWebElement Instance
        {
            get
            {
                return SearchContext;
            }

            set
            {
                SearchContext = value;
                ContainerFactory.InitContainer(this);
            }
        }

        public bool Visible => Instance.Displayed;

        public bool Enabled => Instance.Enabled;

        public string Text => Instance.Text;

        public Point Location => Instance.Location;

        public Rectangle BoundingRectangle => new Rectangle(Location, Instance.Size);

        protected override AppiumWebElement SearchContext
        {
            get
            {
                if (!Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(Locator);
                }

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
            }
        }

        public void SendKeys(string keys) =>
            Instance.SendKeys(keys);

        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);
            Instance.Click();
        }

        public void RightClick()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attribute) =>
            Instance.GetAttribute(attribute);

        public override string ToString() =>
            string.IsNullOrEmpty(Name) ? $"{GetType().Name} [{Locator?.ToString()}]" : Name;
    }
}
