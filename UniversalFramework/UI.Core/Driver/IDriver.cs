using System;

namespace Unicorn.UI.Core.Driver
{
    public interface IDriver : ISearchContext
    {
        void SetImplicitlyWait(TimeSpan time);

        void Get(string path);

        void Close();
    }
}
