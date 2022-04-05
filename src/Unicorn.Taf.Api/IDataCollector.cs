using System.Reflection;

namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Interface for data collectors.
    /// </summary>
    public interface IDataCollector
    {
        /// <summary>
        /// Collects a data from specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">assembly instance</param>
        /// <returns>collected data as <see cref="IOutcome"/></returns>
        IOutcome CollectData(Assembly assembly);
    }
}
