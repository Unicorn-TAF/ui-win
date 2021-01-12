using System;
using System.Threading;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign an author to test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AuthorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorAttribute"/> class with specified author.
        /// </summary>
        /// <param name="author">test author</param>
        public AuthorAttribute(string author)
        {
            Author = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(author);
        }

        /// <summary>
        /// Gets or sets test author.
        /// </summary>
        public string Author { get; }
    }
}
