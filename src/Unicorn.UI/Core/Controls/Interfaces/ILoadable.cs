using System;

namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for loadable components.
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Wait until control is loaded during specified timeout.
        /// </summary>
        /// <param name="timeout">loading wait timeout</param>
        /// <returns>true - if control is loaded before timeout; false - if timed out while waiting for control be loaded</returns>
        bool WaitForLoading(TimeSpan timeout);
    }
}
