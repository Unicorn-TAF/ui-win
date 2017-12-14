using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.Controls
{
    public class WebControl : WebSearchContext, IControl
    {
        public bool Cached = true;

        public WebControl()
        {
        }

        public WebControl(IWebElement instance)
        {
            Instance = instance;
        }

        public ByLocator Locator { get; set; }

        public virtual IWebElement Instance
        {
            get
            {
                return (IWebElement)SearchContext;
            }

            set
            {
                SearchContext = value;
            }
        }

        public string Text
        {
            get
            {
                return Instance.Text;
            }
        }

        public bool Enabled
        {
            get
            {
                return Instance.Enabled;
            }
        }

        public bool Visible
        {
            get
            {
                return Instance.Displayed;
            }
        }

        public Point Location
        {
            get
            {
                return Instance.Location;
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(Location, Instance.Size);
            }
        }

        protected override OpenQA.Selenium.ISearchContext SearchContext
        {
            get
            {
                if (!this.Cached)
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

        public string GetAttribute(string attribute)
        {
            return Instance.GetAttribute(attribute);
        }

        public virtual void Click()
        {
            Instance.Click();
        }

        public void RightClick()
        {
            Actions actions = new Actions((IWebDriver)SearchContext);
            actions.MoveToElement(Instance);
            actions.ContextClick();
            actions.Release().Perform();
        }
    }
}
