using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization.Conditions
{
    /// <summary>
    /// Represents conditions for <see cref="IControl"/> waits.
    /// </summary>
    public static class Until
    {
        /// <summary>
        /// Checks if element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static TTarget Visible<TTarget>(this TTarget element) where TTarget : class, IControl =>
            element.Visible ? element : null;

        /// <summary>
        /// Checks if element is not visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <returns><c>true</c> when element is not visible <c>false</c> otherwise</returns>
        public static TTarget NotVisible<TTarget>(this TTarget element) where TTarget : class, IControl =>
            !element.Visible ? element : null;

        /// <summary>
        /// Checks if element enabled.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <returns><c>true</c> when element enabled and <c>false</c> otherwise</returns>
        public static TTarget Enabled<TTarget>(this TTarget element) where TTarget : class, IControl =>
            element.Enabled ? element : null;

        /// <summary>
        /// Checks if element disabled.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <returns><c>true</c> when element disabled and <c>false</c> otherwise</returns>
        public static TTarget Disabled<TTarget>(this TTarget element) where TTarget : class, IControl =>
            !element.Enabled ? element : null;

        /// <summary>
        /// Checks if element attribute contains expected value.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <param name="attribute">element attribute</param>
        /// <param name="value">attribute value</param>
        /// <returns><c>element</c> when attribute contains expected value and <c>null</c> otherwise</returns>
        public static TTarget AttributeContains<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl =>
            (element as IControl).GetAttribute(attribute).Contains(value) ? element : null;

        /// <summary>
        /// Checks if element attribute does not contain expected value.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <param name="attribute">element attribute</param>
        /// <param name="value">attribute value</param>
        /// <returns><c>element</c> when attribute does not contain expected value and <c>null</c> otherwise</returns>
        public static TTarget AttributeDoesNotContain<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl =>
            !(element as IControl).GetAttribute(attribute).Contains(value) ? element : null;

        /// <summary>
        /// Checks if element attribute has expected value.
        /// </summary>
        /// <typeparam name="TTarget">Target element type implementing <see cref="IControl"/></typeparam>
        /// <param name="element">Target element</param>
        /// <param name="attribute">element attribute</param>
        /// <param name="value">attribute value</param>
        /// <returns><c>element</c> when attribute does not contain expected value and <c>null</c> otherwise</returns>
        public static TTarget AttributeHasValue<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl =>
            (element as IControl).GetAttribute(attribute).Equals(value) ? element : null;
    }
}
