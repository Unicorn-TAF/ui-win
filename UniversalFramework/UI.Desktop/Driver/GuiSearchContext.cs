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
        protected virtual AutomationElement SearchContext { get; set; }

        protected static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);
        protected static TimeSpan ImplicitlyWait = _timeoutDefault;

        private Condition GetClassNameCondition(string className)
        {
            return !string.IsNullOrEmpty(className) ? new PropertyCondition(AutomationElement.ClassNameProperty, className) : Condition.TrueCondition;
        }

        private Condition GetControlTypeCondition(ControlType type)
        {
            return new PropertyCondition(AutomationElement.ControlTypeProperty, type);
        }



        public T Find<T>(ByLocator locator) where T : IControl
        {
            T control = WaitForWrappedControl<T>(locator);

            if (control == null)
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            else
                return control;
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
            ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);
            T control = WaitForWrappedControl<T>(locator);
            ImplicitlyWait = _timeoutDefault;

            controlInstance = control;

            return control != null;
        }


        public T FirstChild<T>() where T : IControl
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            var wrapper = Activator.CreateInstance<T>();

            var condition = new AndCondition(
                TreeWalker.ControlViewWalker.Condition, 
                new PropertyCondition(AutomationElement.ControlTypeProperty, ((GuiControl)(object)wrapper).Type));
            var walker = new TreeWalker(condition);

            var element = walker.GetFirstChild(SearchContext);

            if (element == null)
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");

            ((GuiControl)(object)wrapper).SearchContext = element;

            return wrapper;
        }


        #region "Helpers"


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


        private const int delayBetweenTryes = 100;
        private IList<T> WaitForWrappedControllList<T>(ByLocator locator)
        {
            IList<T> controlsList;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                controlsList = GetWrappedControlsList<T>(locator);
                Thread.Sleep(delayBetweenTryes);
            } while (controlsList.Count == 0 && timer.Elapsed < ImplicitlyWait);

            timer.Stop();

            return controlsList;
        }


        private IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            var aElements = GetNativeControlsList<T>(locator);

            List<T> controlsList = new List<T>();

            foreach (AutomationElement aElement in aElements)
            {
                T wrapper = Activator.CreateInstance<T>();
                ((GuiControl)(object)wrapper).Instance = aElement;
                ((GuiControl)(object)wrapper).ParentContext = SearchContext;
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
                catch(ControlNotFoundException)
                {
                    Thread.Sleep(delayBetweenTryes);
                }
            } while (!success && timer.Elapsed < ImplicitlyWait);

            timer.Stop();

            return control;
        }


        private T GetWrappedControl<T>(ByLocator locator)
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            AutomationElement aElement = GetNativeControl<T>(locator);

            T wrapper = Activator.CreateInstance<T>();
            ((GuiControl)(object)wrapper).Instance = aElement;
            ((GuiControl)(object)wrapper).ParentContext = SearchContext;

            return wrapper;
        }


        protected AutomationElement GetNativeControl<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator<T>(locator);

            AutomationElement nativeControl = SearchContext.FindFirst(TreeScope.Descendants, condition);

            if (nativeControl == null)
                throw new ControlNotFoundException($"Unable to find control by {locator}");

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

            GuiControl _instance = (GuiControl)Activator.CreateInstance(type);

            Condition classCondition = GetClassNameCondition(_instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(_instance.Type);

            _instance = null;

            Condition condition = new AndCondition(classCondition, typeCondition, locatorCondition);

            //Condition condition = GetNativeLocator<T>(locator);

            AutomationElement nativeControl = ParentContext.FindFirst(TreeScope.Descendants, condition);

            if (nativeControl == null)
                throw new ControlNotFoundException($"Unable to find control by {locator}");

            return nativeControl;
        }


        private AutomationElementCollection GetNativeControlsList<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator<T>(locator);
            AutomationElementCollection aElements = SearchContext.FindAll(TreeScope.Descendants, condition);
            return aElements;
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

            GuiControl _instance = (GuiControl)(object)Activator.CreateInstance<T>();

            Condition classCondition = GetClassNameCondition(_instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(_instance.Type);

            _instance = null;

            return new AndCondition(classCondition, typeCondition, locatorCondition);
        }

        #endregion
    }
}
