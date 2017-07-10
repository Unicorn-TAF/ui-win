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

        private static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);
        protected static TimeSpan ImplicitlyWait = _timeoutDefault;

        private Condition GetClassNameCondition(string className) { return !string.IsNullOrEmpty(className) ? new PropertyCondition(AutomationElement.ClassNameProperty, className) : Condition.TrueCondition; }

        private Condition GetControlTypeCondition(ControlType type) { return new PropertyCondition(AutomationElement.ControlTypeProperty, type);  }


        public T FindControl<T>(By by, string locator) where T : IControl
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            return WaitFor<T>(GetCondition(by, locator));
        }


        public IList<T> FindControls<T>(By by, string locator) where T : IControl
        {
            List<T> list = new List<T>();
            var wrapper = Activator.CreateInstance<T>();

            var elementsList = SearchContext.FindAll(TreeScope.Descendants, new AndCondition(
                GetClassNameCondition(((GuiControl)(object)wrapper).ClassName),
                GetControlTypeCondition(((GuiControl)(object)wrapper).Type),
                GetCondition(by, locator)));

            foreach (AutomationElement aElement in elementsList)
            {
                var wrp = Activator.CreateInstance<T>();
                ((GuiControl)(object)wrapper).SearchContext = aElement;
                list.Add(wrp);
            }

            return list;
        }


        private Condition GetCondition(By by, string locator)
        {
            switch(by)
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


        public T FindControl<T>(string name, string alternativeName = null) where T : IControl
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            return WaitFor<T>(name, alternativeName);
        }

        public bool IsControlPresent<T>(By by, string locator) where T : IControl
        {
            if (!typeof(GuiControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            bool isPresented = true;

            ImplicitlyWait = TimeSpan.FromSeconds(0);

            try
            {
                WaitFor<T>(GetCondition(by, locator));
            }
            catch
            {
                isPresented = false;
            }

            ImplicitlyWait = _timeoutDefault;
            return isPresented;
        }

        // Getters

        private T Get<T>(Condition condition)
            where T : IControl
        {
            var wrapper = Activator.CreateInstance<T>();

            var element = SearchContext.FindFirst(TreeScope.Descendants, new AndCondition(
                GetClassNameCondition(((GuiControl)(object)wrapper).ClassName),
                GetControlTypeCondition(((GuiControl)(object)wrapper).Type),
                condition));

            if (element == null)
                throw new ControlNotFoundException($"Unable to find control by {condition}");

            ((GuiControl)(object)wrapper).SearchContext = element;
            return wrapper;
        }


        private T Get<T>(AutomationProperty property, object value)
            where T : GuiControl
        {
            return Get<T>(new PropertyCondition(property, value));
        }


        private T Get<T>(string name, string alternativaName = null)
            where T : IControl
        {
            List<PropertyCondition> conditionsList = new List<PropertyCondition>();
            conditionsList.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, name));
            conditionsList.Add(new PropertyCondition(AutomationElement.NameProperty, name));

            if (!string.IsNullOrEmpty(alternativaName))
            {
                conditionsList.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, alternativaName));
                conditionsList.Add(new PropertyCondition(AutomationElement.NameProperty, alternativaName));
            }
            return Get<T>(new OrCondition(conditionsList.ToArray()));
        }



        // Waiters

        private const int delayBetweenTryes = 100;
        private T WaitFor<T>(string name, string alternativeName) where T : IControl
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            T result = default(T);
            do
            {
                try
                {
                    result = Get<T>(name, alternativeName);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            } while (timer.Elapsed < ImplicitlyWait);
            timer.Stop();

            if (timer.Elapsed > ImplicitlyWait)
                throw new ControlNotFoundException($"Unable to find control with name '{name}' or id '{alternativeName}'");

            return result;
        }

        private T WaitFor<T>(Condition condition) where T : IControl
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            T result = default(T);
            do
            {
                try
                {
                    result = Get<T>(condition);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            } while (timer.Elapsed < ImplicitlyWait);
            timer.Stop();

            if (timer.Elapsed > ImplicitlyWait)
                throw new ControlNotFoundException($"Unable to find control by {condition}");

            return result;
        }

        // Childs

        /*public T FirstChild<T>() where T : GuiControl, new()
        {
            var wrapper = Activator.CreateInstance<T>();

            var condition = new AndCondition(TreeWalker.ControlViewWalker.Condition, new PropertyCondition(AutomationElement.ControlTypeProperty, wrapper.Type));
            var walker = new TreeWalker(condition);

            var element = walker.GetFirstChild(Instance);

            if (element == null)
                return null;

            wrapper.Instance = element;
            return wrapper;
        }*/

        /*public T Next<T>() where T : Control, new()
        {
            var wrapper = Activator.CreateInstance<T>();

            var condition = new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, this.Instance.Current.ProcessId), TreeWalker.ControlViewWalker.Condition, new PropertyCondition(AutomationElement.ControlTypeProperty, wrapper.Type));
            var walker = new TreeWalker(condition);
            var parentWalker = new TreeWalker(new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, this.Instance.Current.ProcessId), TreeWalker.ControlViewWalker.Condition));

            if (this.instance == walker.GetLastChild(parentWalker.GetParent(this.instance)))
                return null;
            var element = walker.GetNextSibling(Instance);

            if (element == null)
                return null;

            wrapper.Instance = element;
            return wrapper;
        }

        public T FindChild<T>(string name) where T : Control, new()
        {
            var item = this.FirstChild<T>();
            while (item != null && !item.Name.Contains(name))
                item = item.Next<T>();
            return item;
        }*/
    }
}
