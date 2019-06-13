using System;
using System.Windows;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.UserInput;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UI.Desktop.Controls
{
    public abstract class GuiControl : GuiSearchContext, IControl
    {
        protected GuiControl()
        {
        }

        protected GuiControl(AutomationElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual string ClassName => null;

        public abstract ControlType UiaType { get; }

        public virtual AutomationElement Instance
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

        public string Text =>
            this.Instance.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;

        public bool Enabled =>
            (bool)this.Instance.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);

        public bool Visible
        {
            get
            {
                try
                {
                    return !this.Instance.Current.IsOffscreen;
                }
                catch (ElementNotAvailableException)
                {
                    return false;
                }
            }
        }

        public System.Drawing.Point Location =>
            new System.Drawing.Point(this.BoundingRectangle.Location.X, this.BoundingRectangle.Location.Y);

        public System.Drawing.Rectangle BoundingRectangle =>
            (System.Drawing.Rectangle)this.Instance.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

        public override AutomationElement SearchContext
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
                    return (string)this.Instance.GetCurrentPropertyValue(AutomationElement.ClassNameProperty);
                case "text":
                    return (string)this.Instance.GetCurrentPropertyValue(AutomationElement.NameProperty);
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
            Logger.Instance.Log(LogLevel.Debug, "Click " + this);
            object pattern = null;

            try
            {
                if (this.Instance.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
                {
                    ((InvokePattern)pattern).Invoke();
                }
                else
                {
                    ((TogglePattern)this.Instance.GetCurrentPattern(TogglePattern.Pattern)).Toggle();
                }
            }
            catch
            {
                MouseClick();
            }
        }

        public void MouseClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Mouse click " + this);
            Point point;
            if (!this.Instance.TryGetClickablePoint(out point))
            {
                Point pt = new Point(3, 3);
                var rect = (Rect)this.Instance.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                point = rect.TopLeft;
                point.Offset(pt.X, pt.Y);
            }

            Mouse.Instance.Click(point);
        }

        public void RightClick()
        {
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this);
            Point point;
            if (!this.Instance.TryGetClickablePoint(out point))
            {
                Point pt = new Point(3, 3);
                var rect = (Rect)this.Instance.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                point = rect.TopLeft;
                point.Offset(pt.X, pt.Y);
            }

            Mouse.Instance.RightClick(point);
        }

        public AutomationElement GetParent() =>
            TreeWalker.ControlViewWalker.GetParent(this.Instance);

        public override string ToString() => 
            string.IsNullOrEmpty(this.Name) ? $"{this.GetType().Name} [{this.Locator?.ToString()}]" : this.Name;

        #region "Helpers"

        protected T GetPattern<T>() where T : BasePattern
        {
            var pattern = (AutomationPattern)typeof(T).GetField("Pattern").GetValue(null);
            object patternObject;
            this.Instance.TryGetCurrentPattern(pattern, out patternObject);
            return patternObject as T;
        }

        #endregion
    }
}
