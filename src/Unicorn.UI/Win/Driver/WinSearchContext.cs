using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UIAutomationClient;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Win.Controls;

namespace Unicorn.UI.Win.Driver
{
    /// <summary>
    /// Describes search context for windows controls. Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public abstract class WinSearchContext : BaseSearchContext<WinSearchContext>
    {
        private const int SearchDelay = 50;

        /// <summary>
        /// Gets or sets Current search context as <see cref="IUIAutomationElement"/>
        /// </summary>
        public virtual IUIAutomationElement SearchContext { get; set; }

        /// <summary>
        /// Gets or sets implicitly wait timeout
        /// </summary>
        protected static TimeSpan ImplicitlyWaitTimeout { get; set; }

        /// <summary>
        /// Gets or sets base <see cref="Type"/> for all desktop controls (by default is <see cref="WinControl"/>)
        /// </summary>
        protected override Type ControlsBaseType => typeof(WinControl);

        #region "Helpers"

        /// <summary>
        /// Wait for typified control by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WinControl"/></typeparam>
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
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WinControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped controls list</returns>
        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList<T>(locator);
            List<T> controlsList = new List<T>();

            for (int i = 0; i < elementsToWrap.Length; i++)
            {
                controlsList.Add(Wrap<T>(elementsToWrap.GetElement(i), null));
            }

            return controlsList;
        }

        /// <summary>
        /// Get first child from current context which has specified control type ignoring implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WinControl"/></typeparam>
        /// <returns>wrapped control instance</returns>
        protected override T GetFirstChildWrappedControl<T>()
        {
            WinControl instance = (WinControl)Activator.CreateInstance(typeof(T));

            var condition = WinDriver.Instance.Driver.CreateAndCondition(
               WinDriver.Instance.Driver.ControlViewCondition,
               GetControlTypeCondition(instance.UiaType));

            var walker = WinDriver.Instance.Driver.CreateTreeWalker(condition);
            var elementToWrap = walker.GetFirstChildElement(SearchContext);

            if (elementToWrap == null)
            {
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");
            }

            return Wrap<T>(elementToWrap, null);
        }

        /// <summary>
        /// Get control instance from current context as UIA <see cref="IUIAutomationElement"/>.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WinControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="IUIAutomationElement"/> instance</returns>
        protected IUIAutomationElement GetNativeControl<T>(ByLocator locator) =>
            GetNativeControlFromContext(locator, typeof(T), SearchContext);

        /// <summary>
        /// Get control instance from parent context as UIA <see cref="IUIAutomationElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <param name="type">control type</param>
        /// <returns><see cref="IUIAutomationElement"/> instance</returns>
        protected IUIAutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type) =>
            GetNativeControlFromContext(locator, type, ParentSearchContext.SearchContext);

        /// <summary>
        /// Set current implicitly wait timeout value.
        /// </summary>
        /// <param name="timeout">new implicit timeout value</param>
        protected override void SetImplicitlyWait(TimeSpan timeout) =>
            ImplicitlyWaitTimeout = timeout;

        private IUIAutomationElement GetNativeControlFromContext(ByLocator locator, Type type, IUIAutomationElement context)
        {
            IUIAutomationCondition condition = GetNativeLocator(locator, type);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            IUIAutomationElement control = null;

            control = context.FindFirst(TreeScope.TreeScope_Descendants, condition);

            while (control == null && timer.Elapsed < ImplicitlyWaitTimeout)
            {
                Thread.Sleep(SearchDelay);
                control = context.FindFirst(TreeScope.TreeScope_Descendants, condition);
            }

            timer.Stop();

            if (control == null)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }

            return control;
        }

        private IUIAutomationElementArray GetNativeControlsList<T>(ByLocator locator)
        {
            IUIAutomationCondition condition = GetNativeLocator(locator, typeof(T));
            IUIAutomationElementArray wrappedElements = SearchContext.FindAll(TreeScope.TreeScope_Descendants, condition);
            return wrappedElements;
        }

        private IUIAutomationCondition GetClassNameCondition(string className)
        {
            return !string.IsNullOrEmpty(className) ? 
                WinDriver.Instance.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, className) : 
                WinDriver.Instance.Driver.CreateTrueCondition();
        }

        private IUIAutomationCondition GetControlTypeCondition(int type)
        {
            return type == 0 ?
                WinDriver.Instance.Driver.CreateTrueCondition() :
                WinDriver.Instance.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, type);
        }

        private IUIAutomationCondition GetNativeLocator(ByLocator locator, Type controlType)
        {
            IUIAutomationCondition locatorCondition = null;

            switch (locator.How)
            {
                case Using.Id:
                    locatorCondition = WinDriver.Instance.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, locator.Locator);
                    break;
                case Using.Class:
                    locatorCondition = WinDriver.Instance.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, locator.Locator);
                    break;
                case Using.Name:
                    locatorCondition = WinDriver.Instance.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, locator.Locator);
                    break;
                default:
                    throw new ArgumentException($"Incorrect locator type specified: {locator.How}");
            }

            WinControl instance = (WinControl)Activator.CreateInstance(controlType);

            IUIAutomationCondition classCondition = GetClassNameCondition(instance.ClassName);
            IUIAutomationCondition typeCondition = GetControlTypeCondition(instance.UiaType);

            var baseAndCondition = WinDriver.Instance.Driver.CreateAndCondition(classCondition, typeCondition);
            return WinDriver.Instance.Driver.CreateAndCondition(baseAndCondition, locatorCondition);
        }

        private T Wrap<T>(IUIAutomationElement elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((WinControl)(object)wrapper).Instance = elementToWrap;
            ((WinControl)(object)wrapper).ParentSearchContext = this;
            ((WinControl)(object)wrapper).Locator = locator;
            return wrapper;
        }
        
        #endregion
    }
}
