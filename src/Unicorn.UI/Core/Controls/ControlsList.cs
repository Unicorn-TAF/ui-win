using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls
{
    /// <summary>
    /// Describes typified list of controls (should implement <see cref="IControl"/>) with dynamic search procedure 
    /// is triggered each time the collection is accessed.
    /// </summary>
    /// <typeparam name="T">contol type (should implement <see cref="IControl"/>)</typeparam>
    public class ControlsList<T> : IList<T> where T : IControl
    {
        private readonly object _parentContext;
        private readonly ByLocator _locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsList"/> class for parent search context and locator.
        /// </summary>
        /// <param name="parentContext">parent context to search controls list from</param>
        /// <param name="locator">locator to search controls list by</param>
        public ControlsList(object parentContext, ByLocator locator)
        {
            _parentContext = parentContext;
            _locator = locator;
        }

        /// <summary>
        /// Gets control from list by its index. The control could not be set.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>element located at specified index</returns>
        /// <exception cref="NotImplementedException">is thrown in case of set attempt</exception>
        public T this[int index] { get => Items[index]; set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets count of controls currently located in list.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Controls list is readonly.
        /// </summary>
        public bool IsReadOnly => true;

        private IList<T> Items =>
            ((ISearchContext)_parentContext).FindList<T>(_locator);

        void ICollection<T>.Add(T item) =>
            throw new NotImplementedException();

        void ICollection<T>.Clear() =>
            throw new NotImplementedException();

        /// <summary>
        /// Gets a value indicating whether list contains specified control or not.
        /// </summary>
        /// <param name="item">control instance</param>
        /// <returns>true - if list contains control instance; otherwise - false</returns>
        public bool Contains(T item) =>
            Items.Contains(item);

        /// <summary>
        /// Copies the elements of controls list to an <see cref="Array"/>, 
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">target array to copy to, must have zero-based indexing</param>
        /// <param name="arrayIndex">the zero-based index in array at which copying begins</param>
        public void CopyTo(T[] array, int arrayIndex) =>
            Items.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() =>
            Items.GetEnumerator();

        /// <summary>
        /// Determines the index of a specific item in the controls list.
        /// </summary>
        /// <param name="item">control instance</param>
        /// <returns>The index of item if found in the list; otherwise, -1</returns>
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
