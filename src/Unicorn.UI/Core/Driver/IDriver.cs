using System;

namespace Unicorn.UI.Core.Driver
{
    /// <summary>
    /// Interface for IU driver
    /// </summary>
    public interface IDriver : ISearchContext
    {
        /// <summary>
        /// Gets or sets implicitly wait property (how much to wait for control appearance when search for it)
        /// </summary>
        TimeSpan ImplicitlyWait { get; set; }
    }
}
