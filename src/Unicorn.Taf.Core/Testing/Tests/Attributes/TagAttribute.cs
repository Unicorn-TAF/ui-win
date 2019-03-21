using System;

namespace Unicorn.Taf.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(string tag)
        {
            this.Tag = tag;
        }

        public string Tag { get; protected set; }
    }
}
