using System;
using System.Collections.Generic;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Driver
{
    public abstract class UISearchContext : ISearchContext
    {
        protected static TimeSpan implicitlyWaitTimeout = TimeoutDefault;

        protected static TimeSpan TimeoutDefault => TimeSpan.FromSeconds(20);

        protected abstract Type ControlsBaseType
        {
            get;
        }

        public T Find<T>(ByLocator locator) where T : IControl
        {
            return WaitForWrappedControl<T>(locator);
        }

        ////T Find<T>(string name, string alternativeName) where T : IControl;

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
            SetImplicitlyWait(TimeSpan.FromMilliseconds(millisecondsTimeout));

            bool isPresented = true;

            try
            {
                controlInstance = Find<T>(locator);
            }
            catch (ControlNotFoundException)
            {
                controlInstance = default(T);
                isPresented = false;
            }

            SetImplicitlyWait(TimeoutDefault);

            return isPresented;
        }

        public T FirstChild<T>() where T : IControl
        {
            throw new NotImplementedException();
        }

        protected abstract T WaitForWrappedControl<T>(ByLocator locator);

        protected abstract IList<T> GetWrappedControlsList<T>(ByLocator locator);

        protected abstract void SetImplicitlyWait(TimeSpan timeout);

        protected void CheckForControlType<T>()
        {
            Type targetControlType = typeof(T);
            if (!this.ControlsBaseType.IsAssignableFrom(targetControlType))
            {
                throw new ArgumentException($"Illegal type of control: {targetControlType}");
            }
        }
    }
}
