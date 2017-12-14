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
    public abstract class GuiSearchContext : ISearchContext
    {
        public AutomationElement ParentContext;
        protected static TimeSpan timeoutDefault = TimeSpan.FromSeconds(20);
        protected static TimeSpan implicitlyWaitTimeout = timeoutDefault;
        private const int SearchDelay = 100;

        protected virtual AutomationElement SearchContext { get; set; }

        public T Find<T>(ByLocator locator) where T : IControl
        {
            T control = WaitForWrappedControl<T>(locator);

            if (control == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
            else
            {
                return control;
            }
        }

        public IList<T> FindList<T>(ByLocator locator) where T : IControl
        {
            return GetWrappedControlsList<T>(locator);
        }

        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout) where T : IControl
        {
            T control;
            return WaitFor<T>(locator, millisecondsTimeout, out control);
        }

        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            implicitlyWaitTimeout = TimeSpan.FromMilliseconds(millisecondsTimeout);
            T control = WaitForWrappedControl<T>(locator);
            implicitlyWaitTimeout = timeoutDefault;

            controlInstance = control;

            return control != null;
        }

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

        protected AutomationElement GetNativeControl<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator<T>(locator);

            AutomationElement nativeControl = this.SearchContext.FindFirst(TreeScope.Descendants, condition);

            if (nativeControl == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }

            return nativeControl;
        }

        protected AutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type)
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
                    throw new ArgumentException($"Incorrect locator type: {locator.How}");
            }

            GuiControl instance = (GuiControl)Activator.CreateInstance(type);

            Condition classCondition = GetClassNameCondition(instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(instance.Type);

            instance = null;

            Condition condition = new AndCondition(classCondition, typeCondition, locatorCondition);

            ////Condition condition = GetNativeLocator<T>(locator);

            AutomationElement nativeControl = this.ParentContext.FindFirst(TreeScope.Descendants, condition);

            if (nativeControl == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }

            return nativeControl;
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

        private IList<T> WaitForWrappedControllList<T>(ByLocator locator)
        {
            IList<T> controlsList;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                controlsList = GetWrappedControlsList<T>(locator);
                Thread.Sleep(SearchDelay);
            }
            while (controlsList.Count == 0 && timer.Elapsed < implicitlyWaitTimeout);

            timer.Stop();

            return controlsList;
        }

        private IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Illegal type of control: " + typeof(T));
            }

            var wrappedElements = GetNativeControlsList<T>(locator);

            List<T> controlsList = new List<T>();

            foreach (AutomationElement wrappedElement in wrappedElements)
            {
                T wrapper = Activator.CreateInstance<T>();
                ((GuiControl)(object)wrapper).Instance = wrappedElement;
                ((GuiControl)(object)wrapper).ParentContext = this.SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        private T WaitForWrappedControl<T>(ByLocator locator)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            T control = default(T);
            bool success = false;

            do
            {
                try
                {
                    control = GetWrappedControl<T>(locator);
                    success = true;
                }
                catch (ControlNotFoundException)
                {
                    Thread.Sleep(SearchDelay);
                }
            }
            while (!success && timer.Elapsed < implicitlyWaitTimeout);

            timer.Stop();

            return control;
        }

        private T GetWrappedControl<T>(ByLocator locator)
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Illegal type of control: " + typeof(T));
            }

            AutomationElement wrappedElement = GetNativeControl<T>(locator);

            T wrapper = Activator.CreateInstance<T>();
            ((GuiControl)(object)wrapper).Instance = wrappedElement;
            ((GuiControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }

        private AutomationElementCollection GetNativeControlsList<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator<T>(locator);
            AutomationElementCollection wrappedElements = this.SearchContext.FindAll(TreeScope.Descendants, condition);
            return wrappedElements;
        }

        private Condition GetNativeLocator<T>(ByLocator locator)
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

            GuiControl instance = (GuiControl)(object)Activator.CreateInstance<T>();

            Condition classCondition = GetClassNameCondition(instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(instance.Type);

            instance = null;

            return new AndCondition(classCondition, typeCondition, locatorCondition);
        }

        #endregion
    }
}
