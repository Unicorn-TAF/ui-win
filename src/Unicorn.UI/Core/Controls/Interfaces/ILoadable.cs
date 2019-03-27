using System;

namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface ILoadable
    {
        bool WaitForLoad(TimeSpan timeout);
    }
}
