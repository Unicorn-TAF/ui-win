using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using Unicorn.UICore.Driver;
using Unicorn.UICore.UI;
using Unicorn.UIDesktop.UI;

namespace Unicorn.UIDesktop.Driver
{
    public abstract class GuiSearchContext : ISearchContext
    {
        protected AutomationElement SearchContext;

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



        public T Find<T>(By by, string locator) where T : IControl
        {
            IList<T> controlsList = WaitForDescendantsListByCondition<T>(GetCondition(by, locator));

            if (controlsList.Count > 0)
                return controlsList[0];
            else
                throw new ControlNotFoundException($"Unable to find control by {by} = {locator}");
        }


        public IList<T> FindList<T>(By by, string locator) where T : IControl
        {
            return GetDescendantsListByCondition<T>(GetCondition(by, locator));
        }


        public bool WaitFor<T>(By by, string locator, int millisecondsTimeout) where T : IControl
        {
            ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);
            IList<T> controlsList = WaitForDescendantsListByCondition<T>(GetCondition(by, locator));
            ImplicitlyWait = _timeoutDefault;

            return controlsList.Count > 0;
        }


        public bool WaitFor<T>(By by, string locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);
            IList<T> controlsList = WaitForDescendantsListByCondition<T>(GetCondition(by, locator));
            ImplicitlyWait = _timeoutDefault;

            if (controlsList.Count > 0)
            {
                controlInstance = controlsList[0];
                return true;
            }
            else
            {
                controlInstance = default(T);
                return false;
            }
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


        public T Find<T>(string name, string alternativaName = null) where T : IControl
        {
            Condition condition = GetAlternativeNameCondition(name, alternativaName);
            IList<T> controlsList = WaitForDescendantsListByCondition<T>(condition);

            if (controlsList.Count > 0)
                return controlsList[0];
            else
                throw new ControlNotFoundException($"Unable to find control with name {name} = {alternativaName}");
        }



        #region "Helpers"

        private Condition GetCondition(By by, string locator)
        {
            switch (by)
            {
                case By.Id:
                    return new PropertyCondition(AutomationElement.AutomationIdProperty, locator);
                case By.Class:
                    return new PropertyCondition(AutomationElement.ClassNameProperty, locator);
                case By.Name:
                    return new PropertyCondition(AutomationElement.NameProperty, locator);
            }
            return null;
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


        private const int delayBetweenTryes = 100;
        private IList<T> WaitForDescendantsListByCondition<T>(Condition condition)
        {
            IList<T> controlsList;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                controlsList = GetDescendantsListByCondition<T>(condition);
                Thread.Sleep(delayBetweenTryes);
            } while (controlsList.Count == 0 && timer.Elapsed < ImplicitlyWait);

            timer.Stop();

            return controlsList;
        }


        private IList<T> GetDescendantsListByCondition<T>(Condition condition)
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            GuiControl _instance = (GuiControl)(object)Activator.CreateInstance<T>();

            Condition classCondition = GetClassNameCondition(_instance.ClassName);
            Condition typeCondition = GetControlTypeCondition(_instance.Type);

            var aElements = SearchContext.FindAll(
                TreeScope.Descendants,
                new AndCondition(classCondition, typeCondition, condition)
            );

            _instance = null;

            List<T> controlsList = new List<T>();

            foreach (AutomationElement aElement in aElements)
            {
                T wrapper = Activator.CreateInstance<T>();
                ((GuiControl)(object)wrapper).SearchContext = aElement;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        #endregion
    }
}
