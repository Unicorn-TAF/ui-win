using System;
using System.Windows;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.UserInput;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UI.Win.Controls
{
    /// <summary>
    /// Represents basic abstract windows control. Contains number of main properties and action under the control.
    /// </summary>
    public abstract class WinControl : WinSearchContext, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinControl"/> class.
        /// </summary>
        protected WinControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinControl"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        protected WinControl(IUIAutomationElement instance)
        {
            this.Instance = instance;
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
        public abstract int UiaType { get; }

        /// <summary>
        /// Gets or sets control wrapped instance as <see cref="IUIAutomationElement"/> which is also current search context.
        /// </summary>
        public virtual IUIAutomationElement Instance
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

        /// <summary>
        /// Gets control text.
        /// </summary>
        public virtual string Text =>
            this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;

        /// <summary>
        /// Gets a value indicating whether control is enabled in UI.
        /// </summary>
        public bool Enabled => 
            (bool)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsEnabledPropertyId);

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
                    isVisible = this.Instance.CurrentIsOffscreen == 0;
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
            new System.Drawing.Point(this.BoundingRectangle.Location.X, this.BoundingRectangle.Location.Y);

        /// <summary>
        /// Gets control bounding rectangle as <see cref="System.Drawing.Rectangle"/>
        /// </summary>
        public System.Drawing.Rectangle BoundingRectangle =>
            (System.Drawing.Rectangle)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_BoundingRectanglePropertyId);

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
        public override IUIAutomationElement SearchContext
        {
            get
            {
                if (!this.Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(this.Locator, this.GetType());
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
                    return (string)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ClassNamePropertyId);
                case "text":
                    return (string)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId);
                case "enabled":
                    return this.Enabled.ToString();
                case "visible":
                    return this.Visible.ToString();
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
                var pattern = this.GetPattern(UIA_PatternIds.UIA_InvokePatternId);

                if (pattern != null)
                {
                    ((IUIAutomationInvokePattern)pattern).Invoke();
                }
                else
                {
                    ((IUIAutomationTogglePattern)this.GetPattern(UIA_PatternIds.UIA_TogglePatternId)).Toggle();
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
            Point clickPoint;
            tagPOINT point;
            if (this.Instance.GetClickablePoint(out point) == 0)
            {
                Point pt = new Point(3, 3);
                var rect = (Rect)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_BoundingRectanglePropertyId);

                clickPoint = new Point(rect.TopLeft.X, rect.TopLeft.Y);
                clickPoint.Offset(pt.X, pt.Y);
            }
            else
            {
                clickPoint = new Point(point.x, point.y);
            }

            Mouse.Instance.Click(clickPoint);
        }

        /// <summary>
        /// Performs right click by mouse on the control by it's coordinates on screen.
        /// </summary>
        public void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this);
            Point clickPoint; 
            tagPOINT point;
            if (this.Instance.GetClickablePoint(out point) == 0)
            {
                Point pt = new Point(3, 3);
                var rect = (Rect)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_BoundingRectanglePropertyId);

                clickPoint = new Point(rect.TopLeft.X, rect.TopLeft.Y);
                clickPoint.Offset(pt.X, pt.Y);
            }
            else
            {
                clickPoint = new Point(point.x, point.y);
            }

            Mouse.Instance.RightClick(clickPoint);
        }

        /// <summary>
        /// Gets parent control as <see cref="IUIAutomationElement"/>.
        /// </summary>
        /// <returns>parent of the control as <see cref="IUIAutomationElement"/></returns>
        public IUIAutomationElement GetParent()
        {
            var treeWalker = WinDriver.Driver.CreateTreeWalker(WinDriver.Driver.ControlViewCondition);
            return treeWalker.GetParentElement(this.Instance);
        }

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() =>
            string.IsNullOrEmpty(this.Name) ? 
            $"{this.GetType().Name} [{this.Locator?.ToString()}]" : 
            this.Name;

        #region "Helpers"

        /// <summary>
        /// Get specified pattern from the control.
        /// </summary>
        /// <param name="patternId">pattern ID</param>
        /// <returns>pattern instance</returns>
        protected object GetPattern(int patternId) =>
            this.Instance.GetCurrentPattern(patternId);

        #endregion
    }
}
