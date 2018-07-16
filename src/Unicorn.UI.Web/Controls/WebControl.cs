using System.Drawing;
using OpenQA.Selenium.Interactions;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;
using Unicorn.Core.Logging;

namespace Unicorn.UI.Web.Controls
{
    public class WebControl : WebSearchContext, IControl
    {
        public WebControl()
        {
        }

        public WebControl(OpenQA.Selenium.IWebElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual OpenQA.Selenium.IWebElement Instance
        {
            get
            {
                return (OpenQA.Selenium.IWebElement)this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
            }
        }

        public string Text => this.Instance.Text;

        public bool Enabled => this.Instance.Enabled;

        public bool Visible => this.Instance.Displayed;

        public Point Location => this.Instance.Location;

        public Rectangle BoundingRectangle => new Rectangle(this.Location, this.Instance.Size);

        protected override OpenQA.Selenium.ISearchContext SearchContext
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

        public string GetAttribute(string attribute)
        {
            return this.Instance.GetAttribute(attribute);
        }

        public virtual void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this.ToString());
            this.Instance.Click();
        }

        public virtual void JsClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "JavaScript click " + this.ToString());
            WebDriver.Instance.ExecuteJS("arguments[0].click()", this.Instance);
        }

        public virtual void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this.ToString());
            Actions actions = new Actions((OpenQA.Selenium.IWebDriver)this.SearchContext);
            actions.MoveToElement(this.Instance);
            actions.ContextClick();
            actions.Release().Perform();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? $"{this.GetType().Name} [{this.Locator?.ToString()}]" : this.Name;
        }
    }
}
