using System;
using System.Threading;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorAttribute : Attribute
    {
        private string author;

        public AuthorAttribute(string author)
        {
            this.author = author;

            this.author = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(author.ToLower());
        }

        public string Author => this.author;
    }
}
