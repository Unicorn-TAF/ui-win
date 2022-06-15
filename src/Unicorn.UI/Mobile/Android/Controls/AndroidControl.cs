using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.Controls
{
    /// <summary>
    /// Represents basic android control. Contains number of main properties and action under the control.
    /// </summary>
    public class AndroidControl : AndroidSearchContext, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidControl"/> class.
        /// </summary>
        public AndroidControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidControl"/> class with wraps 
        /// specific <see cref="AppiumWebElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AppiumWebElement"/> instance to wrap</param>
        public AndroidControl(AppiumWebElement instance)
        {
            Instance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to cache the control.
        /// Cached control is not searched for on each next call. 
        /// Not cached control is searched for each time (as PageObject control).
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
        /// Gets or sets control wrapped instance as <see cref="AppiumWebElement"/> which is also current search context.
        /// </summary>
        public AppiumWebElement Instance
        {
            get => SearchContext;

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
        /// Gets a value indicating whether control is visible
        /// </summary>
        public bool Visible => Instance.Displayed;

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        public Point Location => Instance.Location;

        /// <summary>
        /// Gets control bounding rectangle as <see cref="Rectangle"/>
        /// </summary>
        public Rectangle BoundingRectangle => new Rectangle(Location, Instance.Size);

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
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

            set => base.SearchContext = value;
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
        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);
            Instance.Click();
        }

        /// <summary>
        /// Adds text to already existing input value.
        /// </summary>
        /// <param name="keys">text to send</param>
        public void SendKeys(string keys) =>
            Instance.SendKeys(keys);

        /// <summary>
        /// Right click is not applicable for android.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void RightClick() =>
            throw new NotImplementedException();

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() =>
            string.IsNullOrEmpty(Name) ? $"{GetType().Name} [{Locator?.ToString()}]" : Name;
    }
}
