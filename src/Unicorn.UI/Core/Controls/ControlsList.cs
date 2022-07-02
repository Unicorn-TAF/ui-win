using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    public class ControlsList<T> : IList<T> where T : IControl
    {
        private readonly object _parentContext;
        private readonly ByLocator _locator;

        public ControlsList(object parentContext, ByLocator locator)
        {
            _parentContext = parentContext;
            _locator = locator;
        }

        public T this[int index] { get => Items[index]; set => throw new NotImplementedException(); }

        public int Count => Items.Count;

        public bool IsReadOnly => true;

        private IList<T> Items =>
            ((ISearchContext)_parentContext).FindList<T>(_locator);

        void ICollection<T>.Add(T item) =>
            throw new NotImplementedException();

        void ICollection<T>.Clear() =>
            throw new NotImplementedException();

        public bool Contains(T item) =>
            Items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) =>
            Items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() =>
            Items.GetEnumerator();

        public int IndexOf(T item) =>
            Items.IndexOf(item);

        void IList<T>.Insert(int index, T item) =>
            throw new NotImplementedException();

        bool ICollection<T>.Remove(T item) =>
            throw new NotImplementedException();

        void IList<T>.RemoveAt(int index) =>
            throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)Items).GetEnumerator();
    }
}
