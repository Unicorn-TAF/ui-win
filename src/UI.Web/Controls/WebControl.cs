using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.Controls
{
    public class WebControl : WebSearchContext, IControl
    {
        private bool cached = true;

        public WebControl()
        {
        }

        public WebControl(IWebElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached
        {
            get
            {
                return this.cached;
            }

            set
            {
                this.cached = value;
            }
        }

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual IWebElement Instance
        {
            get
            {
                return (IWebElement)this.SearchContext;
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
            this.Instance.Click();
        }

        public void RightClick()
        {
            Actions actions = new Actions((IWebDriver)this.SearchContext);
            actions.MoveToElement(this.Instance);
            actions.ContextClick();
            actions.Release().Perform();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? base.ToString(): this.Name;
        }
    }
}
