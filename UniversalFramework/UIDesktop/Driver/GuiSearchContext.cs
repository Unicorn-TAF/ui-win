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

        protected TimeSpan ImplicitlyWait = TimeSpan.FromSeconds(20);

        private Condition GetClassNameCondition(string className) { return !string.IsNullOrEmpty(className) ? new PropertyCondition(AutomationElement.ClassNameProperty, className) : Condition.TrueCondition; }
        private Condition GetControlTypeCondition(ControlType type) { return new PropertyCondition(AutomationElement.ControlTypeProperty, type);  }



        public T GetElement<T>(string locator) where T : IControl
        {
            if (typeof(GuiControl).IsAssignableFrom(typeof(T)))
                return Get<T>(locator);
            else
                throw new ArgumentException("Illegal type of control");
        }

        public T WaitForElement<T>(string locator) where T : IControl
        {
            if (typeof(GuiControl).IsAssignableFrom(typeof(T)))
                return WaitFor<T>(locator);
            else
                throw new ArgumentException("Illegal type of control");
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
                throw new ElementNotFoundException("Element not found");

            ((GuiControl)(object)wrapper).Instance = element;
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
                throw new ElementNotFoundException("Element not found");

            return result;
        }

        private T WaitFor<T>(string name) where T : IControl
        {
            return WaitFor<T>(name, null);
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
