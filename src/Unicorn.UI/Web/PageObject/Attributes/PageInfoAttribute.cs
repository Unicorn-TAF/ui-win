using System;

namespace Unicorn.UI.Web.PageObject.Attributes
{
    /// <summary>
    /// Provides with ability to specify web page additional information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PageInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoAttribute"/> class with specified test relative url and title.
        /// </summary>
        /// <param name="relativeUrl">page relative url</param>
        /// <param name="title">page title</param>
        public PageInfoAttribute(string relativeUrl, string title)
        {
            RelativeUrl = relativeUrl;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoAttribute"/> class with specified test relative url and without title.
        /// </summary>
        /// <param name="relativeUrl">page relative url</param>
        public PageInfoAttribute(string relativeUrl) : this(relativeUrl, null)
        {
        }

        /// <summary>
        /// Gets or sets page relative url
        /// </summary>
        public string RelativeUrl { get; }

        /// <summary>
        /// Gets or sets page title
        /// </summary>
        public string Title { get; }

    }
}
