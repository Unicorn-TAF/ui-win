using System.Drawing;
using OpenQA.Selenium.Interactions;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.Controls
{
    /// <summary>
    /// Represents basic web control control. Contains number of main properties and action under the control.
    /// </summary>
    public class WebControl : WebSearchContext, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebControl"/> class.
        /// </summary>
        public WebControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebControl"/> class with wraps specific <see cref="OpenQA.Selenium.IWebElement"/>
        /// </summary>
        /// <param name="instance"><see cref="OpenQA.Selenium.IWebElement"/> instance to wrap</param>
        public WebControl(OpenQA.Selenium.IWebElement instance)
        {
            Instance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to cache the control.
        /// Cached control is not searched for on each next call. Not cached control is searched each time (as PageObject control).
        /// </summary>
        public bool Cached { get; set; } = true;

        /// <summary>
        /// Gets or sets locator to find control by.
        /// </summary>
        public ByLocator Locator { get; set; }

        /// <summary>
        /// Gets or sets control name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets control wrapped instance as <see cref="OpenQA.Selenium.IWebElement"/> which is also current search context.
        /// </summary>
        public virtual OpenQA.Selenium.IWebElement Instance
        {
            get
            {
                return (OpenQA.Selenium.IWebElement)SearchContext;
            }

            set
            {
                SearchContext = value;
                ContainerFactory.InitContainer(this);
            }
        }

        /// <summary>
        /// Gets control text.
        /// </summary>
        public string Text => Instance.Text;

        /// <summary>
        /// Gets a value indicating whether control is enabled in UI.
        /// </summary>
        public bool Enabled => Instance.Enabled;

        /// <summary>
        /// Gets a value indicating whether control is visible (not is Off-screen)
        /// </summary>
        public bool Visible => Instance.Displayed;

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        public Point Location => Instance.Location;

        /// <summary>
        /// Gets control bounding rectangle as <see cref="System.Drawing.Rectangle"/>
        /// </summary>
        public Rectangle BoundingRectangle => new Rectangle(Location, Instance.Size);

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
        protected override OpenQA.Selenium.ISearchContext SearchContext
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

        /// <summary>
        /// Gets control attribute value as <see cref="string"/>
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <returns>control attribute value as string</returns>
        public string GetAttribute(string attribute) =>
            Instance.GetAttribute(attribute);

        /// <summary>
        /// Perform click on control.
        /// </summary>
        public virtual void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);
            Instance.Click();
        }

        /// <summary>
        /// Perform JavaScript click on control.
        /// </summary>
        public virtual void JsClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "JavaScript click " + this);
            WebDriver.Instance.ExecuteJS("arguments[0].click()", Instance);
        }

        /// <summary>
        /// Perform right click on control.
        /// </summary>
        public virtual void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this);
            var actions = new Actions(WebDriver.Instance.SeleniumDriver);
            actions.MoveToElement(Instance);
            actions.ContextClick();
            actions.Release().Perform();
        }

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() =>
            string.IsNullOrEmpty(Name) ? $"{GetType().Name} [{Locator?.ToString()}]" : Name;
    }
}
