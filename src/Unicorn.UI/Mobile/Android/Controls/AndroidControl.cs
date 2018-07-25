using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.Driver;
using Unicorn.Core.Logging;

namespace Unicorn.UI.Mobile.Android.Controls
{
    public class AndroidControl : AndroidSearchContext, IControl
    {
        public AndroidControl()
        {
        }

        public AndroidControl(AppiumWebElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual AppiumWebElement Instance
        {
            get
            {
                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
            }
        }

        public bool Visible => this.Instance.Displayed;

        public bool Enabled => this.Instance.Enabled;

        public string Text => this.Instance.Text;

        public Point Location => this.Instance.Location;

        public Rectangle BoundingRectangle => new Rectangle(this.Location, this.Instance.Size);

        protected override AppiumWebElement SearchContext
        {
            get
            {
                if (!this.Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(this.Locator);
                }

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
            }
        }

        public void SendKeys(string keys)
        {
            this.Instance.SendKeys(keys);
        }

        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this.ToString());
            this.Instance.Click();
        }

        public void RightClick()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attribute)
        {
            return this.Instance.GetAttribute(attribute);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? $"{this.GetType().Name} [{this.Locator?.ToString()}]" : this.Name;
        }
    }
}
