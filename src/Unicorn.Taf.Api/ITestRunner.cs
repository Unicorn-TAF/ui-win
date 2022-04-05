namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Interface for tests runners implementations.
    /// </summary>
    public interface ITestRunner
    {
        /// <summary>
        /// Runs tests and returns result as <see cref="IOutcome"/>
        /// </summary>
        /// <returns>run outcome</returns>
        IOutcome RunTests();
    }
}
