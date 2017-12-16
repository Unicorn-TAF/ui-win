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
        private const int SearchDelay = 100;

        public AutomationElement ParentContext { get; set; }

        protected virtual AutomationElement SearchContext { get; set; }

        protected override Type ControlsBaseType => typeof(GuiControl);

        public T FirstChild<T>() where T : IControl
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Illegal type of control: " + typeof(T));
            }

            var wrapper = Activator.CreateInstance<T>();

            var condition = new AndCondition(
                TreeWalker.ControlViewWalker.Condition, 
                new PropertyCondition(AutomationElement.ControlTypeProperty, ((GuiControl)(object)wrapper).Type));
            var walker = new TreeWalker(condition);

            var element = walker.GetFirstChild(this.SearchContext);

            if (element == null)
            {
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");
            }

            ((GuiControl)(object)wrapper).SearchContext = element;

            return wrapper;
        }

        #region "Helpers"

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            CheckForControlType<T>();

            AutomationElement elementToWrap = GetNativeControl<T>(locator);

            T wrapper = Activator.CreateInstance<T>();
            ((GuiControl)(object)wrapper).Instance = elementToWrap;
            ((GuiControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            CheckForControlType<T>();

            var elementsToWrap = GetNativeControlsList<T>(locator);

            List<T> controlsList = new List<T>();

            foreach (AutomationElement elementToWrap in elementsToWrap)
            {
                T wrapper = Activator.CreateInstance<T>();
                ((GuiControl)(object)wrapper).Instance = elementToWrap;
                ((GuiControl)(object)wrapper).ParentContext = this.SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        protected AutomationElement GetNativeControl<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator(locator, typeof(T));

            Stopwatch timer = new Stopwatch();
            timer.Start();

            AutomationElement control = null;

            do
            {
                control = this.SearchContext.FindFirst(TreeScope.Descendants, condition);
                Thread.Sleep(SearchDelay);
            }
            while (control == null && timer.Elapsed < UISearchContext.implicitlyWaitTimeout);

            timer.Stop();

            if (control == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
            else
            {
                return control;
            }
        }

        protected AutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type)
        {
            Condition condition = GetNativeLocator(locator, type);

            AutomationElement nativeControl = this.ParentContext.FindFirst(TreeScope.Descendants, condition);

            if (nativeControl == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }

            return nativeControl;
        }

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            UISearchContext.implicitlyWaitTimeout = timeout;
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

        private Condition GetAlternativeNameCondition(string name, string alternativaName)
        {
            List<PropertyCondition> conditionsList = new List<PropertyCondition>();
            conditionsList.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, name));
            conditionsList.Add(new PropertyCondition(AutomationElement.NameProperty, name));

            if (!string.IsNullOrEmpty(alternativaName))
            {
                conditionsList.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, alternativaName));
                conditionsList.Add(new PropertyCondition(AutomationElement.NameProperty, alternativaName));
            }

            return new OrCondition(conditionsList.ToArray());
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

        #endregion
    }
}
