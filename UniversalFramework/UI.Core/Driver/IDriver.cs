using System;

namespace Unicorn.UI.Core.Driver
{
    public interface IDriver : ISearchContext
    {
        TimeSpan ImplicitlyWait { get; set; }

        void Get(string path);
    }
}
