using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Controls;

namespace Unicorn.UI.Desktop.Driver
{
    public abstract class GuiSearchContext : UISearchContext
    {
        private const int SearchDelay = 50;

        public GuiSearchContext ParentSearchContext { get; set; }

        protected static TimeSpan ImplicitlyWaitTimeout { get; set; }

        public virtual AutomationElement SearchContext { get; set; }

        protected override Type ControlsBaseType => typeof(GuiControl);

        #region "Helpers"

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            var elementToWrap = GetNativeControl<T>(locator);
            return this.Wrap<T>(elementToWrap);
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList<T>(locator);
            List<T> controlsList = new List<T>();

            foreach (AutomationElement elementToWrap in elementsToWrap)
            {
                controlsList.Add(this.Wrap<T>(elementToWrap));
            }

            return controlsList;
        }

        protected override T GetFirstChildWrappedControl<T>()
        {
            var condition = new AndCondition(
                TreeWalker.ControlViewWalker.Condition,
                new PropertyCondition(AutomationElement.ControlTypeProperty, typeof(T)));

            var walker = new TreeWalker(condition);
            var elementToWrap = walker.GetFirstChild(this.SearchContext);

            if (elementToWrap == null)
            {
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");
            }

            return this.Wrap<T>(elementToWrap);
        }

        protected AutomationElement GetNativeControl<T>(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, typeof(T), this.SearchContext);
        }

        protected AutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type)
        {
            return GetNativeControlFromContext(locator, type, this.ParentSearchContext.SearchContext);
        }

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            ImplicitlyWaitTimeout = timeout;
        }

        private AutomationElement GetNativeControlFromContext(ByLocator locator, Type type, AutomationElement context)
        {
            Condition condition = GetNativeLocator(locator, type);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            AutomationElement control = null;

            control = context.FindFirst(TreeScope.Descendants, condition);

            while (control == null && timer.Elapsed < ImplicitlyWaitTimeout)
            {
                Thread.Sleep(SearchDelay);
                control = context.FindFirst(TreeScope.Descendants, condition);
            }

            timer.Stop();

            if (control == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }

            return control;
        }

        private AutomationElementCollection GetNativeControlsList<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator(locator, typeof(T));
            AutomationElementCollection wrappedElements = this.SearchContext.FindAll(TreeScope.Descendants, condition);
            return wrappedElements;
        }

        private Condition GetClassNameCondition(string className)
        {
            return !string.IsNullOrEmpty(className) ? new PropertyCondition(AutomationElement.ClassNameProperty, className) : Condition.TrueCondition;
        }

        private Condition GetControlTypeCondition(ControlType type)
        {
            return new PropertyCondition(AutomationElement.ControlTypeProperty, type);
        }

        private Condition GetNativeLocator(ByLocator locator, Type controlType)
        {
            Condition locatorCondition = null;

            switch (locator.How)
            {
                case Using.Id:
                    locatorCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, locator.Locator);
                    break;
                case Using.Class:
                    locatorCondition = new PropertyCondition(AutomationElement.ClassNameProperty, locator.Locator);
                    break;
                case Using.Name:
                    locatorCondition = new PropertyCondition(AutomationElement.NameProperty, locator.Locator);
                    break;
                default:
                    throw new ArgumentException($"Incorrect locator type specified: {locator.How}");
            }

            GuiControl instance = (GuiControl)Activator.CreateInstance(controlType);

            Condition classCondition = GetClassNameCondition(instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(instance.Type);

            instance = null;

            return new AndCondition(classCondition, typeCondition, locatorCondition);
        }

        private T Wrap<T>(AutomationElement elementToWrap)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((GuiControl)(object)wrapper).Instance = elementToWrap;
            ((GuiControl)(object)wrapper).ParentSearchContext = this;

            return wrapper;
        }
        
        #endregion
    }
}
