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
    public abstract class WinSearchContext : UISearchContext<WinSearchContext>
    {
        private const int SearchDelay = 50;

        public virtual IUIAutomationElement SearchContext { get; set; }

        protected static TimeSpan ImplicitlyWaitTimeout { get; set; }

        protected override Type ControlsBaseType => typeof(WinControl);

        #region "Helpers"

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            var elementToWrap = GetNativeControl<T>(locator);
            return this.Wrap<T>(elementToWrap, locator);
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList<T>(locator);
            List<T> controlsList = new List<T>();

            for (int i = 0; i < elementsToWrap.Length; i++)
            {
                controlsList.Add(this.Wrap<T>(elementsToWrap.GetElement(i), null));
            }

            return controlsList;
        }

        protected override T GetFirstChildWrappedControl<T>()
        {
            WinControl instance = (WinControl)Activator.CreateInstance(typeof(T));

            var condition = WinDriver.Driver.CreateAndCondition(
               WinDriver.Driver.ControlViewCondition,
               GetControlTypeCondition(instance.Type));

            var walker = WinDriver.Driver.CreateTreeWalker(condition);
            var elementToWrap = walker.GetFirstChildElement(this.SearchContext);

            if (elementToWrap == null)
            {
                throw new ControlNotFoundException($"Unable to find child {typeof(T)}");
            }

            return this.Wrap<T>(elementToWrap, null);
        }

        protected IUIAutomationElement GetNativeControl<T>(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, typeof(T), this.SearchContext);
        }

        protected IUIAutomationElement GetNativeControlFromParentContext(ByLocator locator, Type type)
        {
            return GetNativeControlFromContext(locator, type, this.ParentSearchContext.SearchContext);
        }

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            ImplicitlyWaitTimeout = timeout;
        }

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
            IUIAutomationElementArray wrappedElements = this.SearchContext.FindAll(TreeScope.TreeScope_Descendants, condition);
            return wrappedElements;
        }

        private IUIAutomationCondition GetClassNameCondition(string className)
        {
            return !string.IsNullOrEmpty(className) ? 
                WinDriver.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, className) : 
                WinDriver.Driver.CreateTrueCondition();
        }

        private IUIAutomationCondition GetControlTypeCondition(int type)
        {
            return WinDriver.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, type);
        }

        private IUIAutomationCondition GetNativeLocator(ByLocator locator, Type controlType)
        {
            IUIAutomationCondition locatorCondition = null;

            switch (locator.How)
            {
                case Using.Id:
                    locatorCondition = WinDriver.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, locator.Locator);
                    break;
                case Using.Class:
                    locatorCondition = WinDriver.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, locator.Locator);
                    break;
                case Using.Name:
                    locatorCondition = WinDriver.Driver.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, locator.Locator);
                    break;
                default:
                    throw new ArgumentException($"Incorrect locator type specified: {locator.How}");
            }

            WinControl instance = (WinControl)Activator.CreateInstance(controlType);

            IUIAutomationCondition classCondition = GetClassNameCondition(instance.ClassName);
            IUIAutomationCondition typeCondition = GetControlTypeCondition(instance.Type);

            var baseAndCondition = WinDriver.Driver.CreateAndCondition(classCondition, typeCondition);
            return WinDriver.Driver.CreateAndCondition(baseAndCondition, locatorCondition);
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
