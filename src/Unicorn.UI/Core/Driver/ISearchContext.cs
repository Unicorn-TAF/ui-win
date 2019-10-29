using System.Collections.Generic;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Driver
{
    /// <summary>
    /// Represent UI search context to search child elements from.
    /// </summary>
    public interface ISearchContext
    {
        /// <summary>
        /// Searches for typified UI control by specified locator from current context during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">control type</typeparam>
        /// <param name="locator">search locator</param>
        /// <returns></returns>
        /// <exception cref="ControlNotFoundException"> is thrown if control is not found</exception>
        T Find<T>(ByLocator locator) where T : IControl;

        IList<T> FindList<T>(ByLocator locator) where T : IControl;

        /// <summary>
        /// Tries to get child control by specified locator imidiately.
        /// </summary>
        /// <typeparam name="T">control type</typeparam>
        /// <param name="locator">search locator</param>
        /// <returns>true - if control was found; otherwise - false</returns>
        bool TryGetChild<T>(ByLocator locator) where T : IControl;

        /// <summary>
        /// Tries to get child control by specified locator during specified timeout in milliseconds.
        /// </summary>
        /// <typeparam name="T">control type</typeparam>
        /// <param name="locator">control locator</param>
        /// <param name="millisecondsTimeout">search timeout (in milliseconds)</param>
        /// <returns>true - if control was found; otherwise - false</returns>
        bool TryGetChild<T>(ByLocator locator, int millisecondsTimeout) where T : IControl;

        /// <summary>
        /// Tries to get child control by specified locator during specified timeout in milliseconds and returns it as <c>out</c> parameter.
        /// </summary>
        /// <typeparam name="T">control type</typeparam>
        /// <param name="locator">control locator</param>
        /// <param name="millisecondsTimeout">search timeount (in milliseconds)</param>
        /// <param name="controlInstance">found control instance (null - if control was not found)</param>
        /// <returns>true - if control was found; otherwise - false</returns>
        bool TryGetChild<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl;
    }
}
