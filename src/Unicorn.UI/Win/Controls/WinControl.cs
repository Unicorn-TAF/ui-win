using System;
using System.Windows;
using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Win.Driver;
using Unicorn.UI.Win.Input;

namespace Unicorn.UI.Win.Controls
{
    public abstract class WinControl : WinSearchContext, IControl
    {
        public WinControl()
        {
        }

        public WinControl(IUIAutomationElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual string ClassName => null;

        public abstract int Type { get; }

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

        public string Text
        {
            get
            {
                var name = this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;
                var id = this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_AutomationIdPropertyId) as string;
                return !string.IsNullOrEmpty(name) ? name : id;
            }
        }

        public bool Enabled => (bool)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsEnabledPropertyId);

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

        public System.Drawing.Point Location => 
            new System.Drawing.Point(this.BoundingRectangle.Location.X, this.BoundingRectangle.Location.Y);

        public System.Drawing.Rectangle BoundingRectangle =>
            (System.Drawing.Rectangle)this.Instance.GetCurrentPropertyValue(UIA_PropertyIds.UIA_BoundingRectanglePropertyId);

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

        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this.ToString());

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

        public void MouseClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Mouse click " + this.ToString());
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

        public void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this.ToString());
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

        public IUIAutomationElement GetParent()
        {
            var treeWalker = WinDriver.Driver.CreateTreeWalker(WinDriver.Driver.ControlViewCondition);
            return treeWalker.GetParentElement(this.Instance);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? $"{this.GetType().Name} [{this.Locator?.ToString()}]" : this.Name;
        }

        #region "Helpers"

        protected object GetPattern(int patternId)
        {
            var pattern = Instance.GetCurrentPattern(patternId);
            return pattern;
        }

        #endregion
    }
}
