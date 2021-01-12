using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents outcome of executed <see cref="TestSuite"/>. Contains next info:<para/>
    /// ID; Suite Name; Execution result; Duration;<para/>
    /// </summary>
    [Serializable]
    public class SuiteOutcome
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteOutcome"/> class.
        /// </summary>
        public SuiteOutcome()
        {
            TestsOutcomes = new List<TestOutcome>();
        }

        /// <summary>
        /// Gets or sets unique across test assembly suite ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets test suite data set name.
        /// </summary>
        public string DataSetName { get; set; }

        /// <summary>
        /// Gets or sets test suite name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets list of suite tests outcomes
        /// </summary>
        public List<TestOutcome> TestsOutcomes { get; }

        /// <summary>
        /// Gets or sets value indicating overall suite execution result
        /// </summary>
        public Status Result { get; set; }

        /// <summary>
        /// Gets or sets suite execution time (sum of executions of all suite methods)
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets total tests matching run filters
        /// </summary>
        public int TotalTests => TestsOutcomes.Count;

        /// <summary>
        /// Gets number of passed tests
        /// </summary>
        public int PassedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Passed));

        /// <summary>
        /// Gets number of failed tests
        /// </summary>
        public int FailedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Failed));

        /// <summary>
        /// Gets number of skipped tests
        /// </summary>
        public int SkippedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Skipped));
    }
}
