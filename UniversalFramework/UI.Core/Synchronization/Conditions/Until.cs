using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization.Conditions
{
    public static class Until
    {
        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static TTarget Visible<TTarget>(this TTarget element) where TTarget : class, IControl
        {
            return element.Visible ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static TTarget Enabled<TTarget>(this TTarget element) where TTarget : class, IControl
        {
            return element.Enabled ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static TTarget AttributeContains<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl
        {
            return (element as IControl).GetAttribute(attribute).Contains(value) ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static TTarget AttributeDoesNotContain<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl
        {
            return !(element as IControl).GetAttribute(attribute).Contains(value) ? element : null;
        }
    }
}
