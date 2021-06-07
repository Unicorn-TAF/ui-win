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
    /// <summary>
    /// Describes search context for windows controls. Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public abstract class GuiSearchContext : BaseSearchContext<GuiSearchContext>
    {
        private const int SearchDelay = 50;

        /// <summary>
        /// Gets or sets Current search context as <see cref="AutomationElement"/>
        /// </summary>
        public virtual AutomationElement SearchContext { get; set; }

        /// <summary>
        /// Gets or sets implicitly wait timeout
        /// </summary>
        protected static TimeSpan ImplicitlyWaitTimeout { get; set; }

        /// <summary>
        /// Gets or sets base <see cref="Type"/> for all desktop controls (by default is <see cref="GuiControl"/>)
        /// </summary>
        protected override Type ControlsBaseType => typeof(GuiControl);

        #region "Helpers"

        /// <summary>
        /// Wait for typified control by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="GuiControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            var elementToWrap = GetNativeControl<T>(locator);
            return Wrap<T>(elementToWrap, locator);
        }

        /// <summary>
        /// Wait for typified controls list by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="GuiControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped controls list</returns>
        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList<T>(locator);
            List<T> controlsList = new List<T>();

            foreach (AutomationElement elementToWrap in elementsToWrap)
            {
                controlsList.Add(Wrap<T>(elementToWrap, null));
            }

            return controlsList;
        }

        /// <summary>
        /// Get first child from current context which has specified control type ignoring implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="GuiControl"/></typeparam>
        /// <returns>wrapped control instance</returns>
        protected override T GetFirstChildWrappedControl<T>()
        {
            GuiControl instance = (GuiControl)Activator.CreateInstance(typeof(T));

            var condition = new AndCondition(
                TreeWalker.ControlViewWalker.Condition,
               GetControlTypeCondition(instance.UiaType));

            var walker = new TreeWalker(condition);
            var elementToWrap = walker.GetFirstChild(SearchContext);

            if (elementToWrap == null)
            {
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");
            }

            return Wrap<T>(elementToWrap, null);
        }

        /// <summary>
        /// Get control instance from current context as UIA <see cref="AutomationElement"/>.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="GuiControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="AutomationElement"/> instance</returns>
        protected AutomationElement GetNativeControl<T>(ByLocator locator) =>
            GetNativeControlFromContext(locator, typeof(T), SearchContext);

        /// <summary>
        /// Get control instance from parent context as UIA <see cref="AutomationElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <param name="type">control type</param>
        /// <returns><see cref="AutomationElement"/> instance</returns>
        protected AutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type) =>
            GetNativeControlFromContext(locator, type, ParentSearchContext.SearchContext);

        /// <summary>
        /// Set current implicitly wait timeout value.
        /// </summary>
        /// <param name="timeout">new implicit timeout value</param>
        protected override void SetImplicitlyWait(TimeSpan timeout) =>
            ImplicitlyWaitTimeout = timeout;

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
                throw new ControlNotFoundException($"Unable to find '{type.Name}' by {locator}");
            }

            return control;
        }

        private AutomationElementCollection GetNativeControlsList<T>(ByLocator locator)
        {
            Condition condition = GetNativeLocator(locator, typeof(T));
            AutomationElementCollection wrappedElements = SearchContext.FindAll(TreeScope.Descendants, condition);
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
            Condition typeCondition = GetControlTypeCondition(instance.UiaType);

            return new AndCondition(classCondition, typeCondition, locatorCondition);
        }

        private T Wrap<T>(AutomationElement elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((GuiControl)(object)wrapper).Instance = elementToWrap;
            ((GuiControl)(object)wrapper).ParentSearchContext = this;
            ((GuiControl)(object)wrapper).Locator = locator;
            return wrapper;
        }
        
        #endregion
    }
}
