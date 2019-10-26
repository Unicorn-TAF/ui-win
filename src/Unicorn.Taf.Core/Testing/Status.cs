namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents execution status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Execution is completed successfully.
        /// </summary>
        Passed,

        /// <summary>
        /// Execution is failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Execution was planned but skipped.
        /// </summary>
        Skipped,

        /// <summary>
        /// Execution was not planned.
        /// </summary>
        NotExecuted
    }
}
