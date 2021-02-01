using System;
using System.Windows;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.UserInput;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.Controls
{
    /// <summary>
    /// Represents basic abstract windows control. Contains number of main properties and action under the control.
    /// </summary>
    public class WinControl : WinSearchContext, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinControl"/> class.
        /// </summary>
        public WinControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinControl"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public WinControl(IUIAutomationElement instance)
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
        /// Gets UI automation element class name.
        /// </summary>
        public virtual string ClassName => null;

        /// <summary>
        /// Gets UI Automation element type.
        /// </summary>
        public virtual int UiaType { get; }

        /// <summary>
        /// Gets or sets control wrapped instance as <see cref="IUIAutomationElement"/> which is also current search context.
        /// </summary>
        public IUIAutomationElement Instance
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

        /// <summary>
        /// Gets control text.
        /// </summary>
        public virtual string Text =>
            Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;

        /// <summary>
        /// Gets a value indicating whether control is enabled in UI.
        /// </summary>
        public bool Enabled => 
            (bool)Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsEnabledPropertyId);

        /// <summary>
        /// Gets a value indicating whether control is visible (not is Off-screen)
        /// </summary>
        public bool Visible
        {
            get
            {
                bool isVisible;
                try
                {
                    isVisible = Instance.CurrentIsOffscreen == 0;
                }
                catch (Exception)
                {
                    isVisible = false;
                }

                return isVisible;
            }
        }

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        public System.Drawing.Point Location => 
            new System.Drawing.Point(BoundingRectangle.Location.X, BoundingRectangle.Location.Y);

        /// <summary>
        /// Gets control bounding rectangle as <see cref="System.Drawing.Rectangle"/>
        /// </summary>
        public System.Drawing.Rectangle BoundingRectangle
        {
            get
            {
                var rect = Instance.CurrentBoundingRectangle;
                return new System.Drawing.Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            }
        }

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
        public override IUIAutomationElement SearchContext
        {
            get
            {
                if (!Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(Locator, GetType());
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
        public string GetAttribute(string attribute)
        {
            switch (attribute.ToLower())
            {
                case "class":
                    return (string)Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ClassNamePropertyId);
                default:
                    throw new ArgumentException($"No such property as {attribute}");
            }
        }

        /// <summary>
        /// Performs click on the control.<para/>
        /// - if control has invoke pattern then control is invoked<para/>
        /// - else if control has toggle pattern then control is toggled<para/>
        /// - else mouse click is performed.
        /// </summary>
        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);

            try
            {
                var pattern = Instance.GetPattern<IUIAutomationInvokePattern>();

                if (pattern != null)
                {
                    pattern.Invoke();
                }
                else
                {
                    Instance.GetPattern<IUIAutomationTogglePattern>().Toggle();
                }
            }
            catch
            {
                MouseClick();
            }
        }

        /// <summary>
        /// Performs mouse click on the control by it's coordinates on screen.
        /// </summary>
        public void MouseClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Mouse click " + this);
            var point = GetClickablePoint();
            Mouse.Instance.Click(point);
        }

        /// <summary>
        /// Performs right click by mouse on the control by it's coordinates on screen.
        /// </summary>
        public void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this);
            var point = GetClickablePoint();
            Mouse.Instance.RightClick(point);
        }

        /// <summary>
        /// Gets parent control as <see cref="IUIAutomationElement"/>.
        /// </summary>
        /// <returns>parent of the control as <see cref="IUIAutomationElement"/></returns>
        public IUIAutomationElement GetParent()
        {
            var treeWalker = WinDriver.Instance.Driver.CreateTreeWalker(WinDriver.Instance.Driver.ControlViewCondition);
            return treeWalker.GetParentElement(Instance);
        }

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() =>
            string.IsNullOrEmpty(Name) ? 
            $"{GetType().Name} [{Locator?.ToString()}]" : 
            Name;

        #region "Helpers"

        private Point GetClickablePoint()
        {
            if (!Visible)
            {
                throw new ControlInvalidStateException("Control is not visible, other control will receive the mouse click.");
            }

            Point point;
            tagPOINT tagPoint;

            if (Instance.GetClickablePoint(out tagPoint) == 0)
            {
                Logger.Instance.Log(LogLevel.Trace, "Clickable point is not found, clicking on center of control...");

                var rect = BoundingRectangle;
                point = new Point(rect.Left, rect.Top);
                point.Offset(rect.Width / 2d, rect.Height / 2d);
            }
            else
            {
                point = new Point(tagPoint.x, tagPoint.y);
            }

            return point;
        }

        #endregion
    }
}
