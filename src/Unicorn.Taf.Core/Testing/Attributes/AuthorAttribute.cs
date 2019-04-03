using System;
using System.Threading;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorAttribute : Attribute
    {
        public AuthorAttribute(string author)
        {
            this.Author = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(author.ToLower());
        }

        public string Author { get; protected set; }
    }
}
