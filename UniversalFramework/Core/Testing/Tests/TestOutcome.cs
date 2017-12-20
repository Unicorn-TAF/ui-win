using System;
using System.Collections.Generic;

namespace Unicorn.Core.Testing.Tests
{
    public class TestOutcome
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestOutcome"/> class. Initializes with zero execution time and empty bugs array
        /// </summary>
        public TestOutcome()
        {
            this.ExecutionTime = TimeSpan.FromSeconds(0);
            this.Bugs = new List<string>();
            this.OpenBugString = string.Empty;
        }

        /// <summary>
        /// Gets or sets Execution Result.
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// Gets or sets Test execution time as TimeSpan.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets Fail Exception details. Has value only when test has failed.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets Screenshot of fail.
        /// </summary>
        public string Screenshot { get; set; }

        /// <summary>
        /// Gets Array of bugs attached to the test. Has values only when the test failed by bug.
        /// </summary>
        public List<string> Bugs { get; }

        /// <summary>
        /// Gets or sets string for open bug
        /// </summary>
        public string OpenBugString { get; set; }
    }
}
