using OpenQA.Selenium;
using Unicorn.UI.Web.Driver;
using System.Drawing;
using OpenQA.Selenium.Interactions;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Web.Controls
{
    public class WebControl : WebSearchContext, IControl {


        private ByLocator _locator;
        public ByLocator Locator
        {
            get
            {
                return _locator;
            }

            set
            {
                _locator = value;
            }
        }

        public bool Cached = true;


        protected override OpenQA.Selenium.ISearchContext SearchContext
        {
            get
            {
                if (!Cached)
                    base.SearchContext = GetNativeControlFromParentContext(Locator);

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
            }
        }


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

        public Size Size
        {
            get
            {
                return Instance.Size;
            }
        }

        public WebControl() { }

        public WebControl(IWebElement instance)
        {
            Instance = instance;
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
