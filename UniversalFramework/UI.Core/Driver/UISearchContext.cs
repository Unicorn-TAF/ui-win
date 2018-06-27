using System;
using System.Collections.Generic;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Driver
{
    public abstract class UISearchContext : ISearchContext
    {
        protected TimeSpan TimeoutDefault => TimeSpan.FromSeconds(30);

        protected abstract Type ControlsBaseType
        {
            get;
        }

        public T Find<T>(ByLocator locator) where T : IControl
        {
            CheckForControlType<T>();
            return WaitForWrappedControl<T>(locator);
        }

        public IList<T> FindList<T>(ByLocator locator) where T : IControl
        {
            CheckForControlType<T>();
            return GetWrappedControlsList<T>(locator);
        }

        public bool TryGetChild<T>(ByLocator locator, int millisecondsTimeout) where T : IControl
        {
            T control;
            return TryGetChild<T>(locator, millisecondsTimeout, out control);
        }

        public bool TryGetChild<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl
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

            SetImplicitlyWait(this.TimeoutDefault);

            return isPresented;
        }

        public T FirstChild<T>() where T : IControl
        {
            CheckForControlType<T>();
            return GetFirstChildWrappedControl<T>();
        }

        protected abstract T WaitForWrappedControl<T>(ByLocator locator);

        protected abstract IList<T> GetWrappedControlsList<T>(ByLocator locator);

        protected abstract T GetFirstChildWrappedControl<T>();

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
