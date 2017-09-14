using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorAttribute : Attribute
    {
        private string _author;

        public AuthorAttribute(string author)
        {
            _author = author;
        }

        public string Author
        {
            get
            {
                return _author;
            }
        }
    }
}
