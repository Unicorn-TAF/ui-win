using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorAttribute : Attribute
    {
        private string author;

        public AuthorAttribute(string author)
        {
            this.author = author;
        }

        public string Author => this.author;
    }
}
