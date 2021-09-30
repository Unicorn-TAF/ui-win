using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to specify execution order for target test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAttribute"/> class with specified order.
        /// </summary>
        /// <param name="order"></param>
        public OrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Gets test order.
        /// </summary>
        public int Order { get; }
    }
}
