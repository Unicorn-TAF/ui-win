using System;
using System.Windows;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Driver;
using Unicorn.UI.Desktop.Input;

namespace Unicorn.UI.Desktop.Controls
{
    public abstract class GuiControl : GuiSearchContext, IControl
    {
        public GuiControl()
        {
        }

        public GuiControl(AutomationElement instance)
        {
            this.Instance = instance;
        }

        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public virtual string ClassName => null;

        public abstract ControlType Type { get; }

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

        public string Text
        {
            get
            {
                var name = this.Instance.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                var id = this.Instance.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty) as string;
                return !string.IsNullOrEmpty(name) ? name : id;
            }
        }

        public bool Enabled
        {
            get
            {
                return (bool)this.Instance.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
            }
        }

        public bool Visible
        {
            get
            {
                bool isVisible;
                try
                {
                    isVisible = !this.Instance.Current.IsOffscreen;
                }
                catch (ElementNotAvailableException)
                {
                    isVisible = false;
                }

                return isVisible;
            }
        }

        public System.Drawing.Point Location
        {
            get
            {
                return new System.Drawing.Point(this.BoundingRectangle.Location.X, this.BoundingRectangle.Location.Y);
            }
        }

        public System.Drawing.Rectangle BoundingRectangle
        {
            get
            {
                return (System.Drawing.Rectangle)this.Instance.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
            }
        }

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
            AutomationProperty ap;

            switch (attribute.ToLower())
            {
                case "class":
                    ap = AutomationElement.ClassNameProperty;
                    break;
                case "text":
                    ap = AutomationElement.NameProperty;
                    break;
                case "enabled":
                    return this.Enabled.ToString();
                case "visible":
                    return this.Visible.ToString();
                default:
                    throw new ArgumentException($"No such property as {attribute}");
            }

            return (string)this.Instance.GetCurrentPropertyValue(ap);
        }

        public void Click()
        {
            Logger.Instance.Log(LogLevel.Debug, "Click " + this.ToString());
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
            Logger.Instance.Log(LogLevel.Debug, "Mouse click " + this.ToString());
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
            Logger.Instance.Log(LogLevel.Debug, "Right click " + this.ToString());
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

        public AutomationElement GetParent()
        {
            TreeWalker treeWalker = TreeWalker.ControlViewWalker;
            return treeWalker.GetParent(this.Instance);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? $"{this.GetType().Name} [{this.Locator.ToString()}]" : this.Name;
        }

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
