using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Tests
{
    [Serializable]
    public class SuiteOutcome
    {
        public SuiteOutcome()
        {
            this.Bugs = new HashSet<string>();
            this.TestsOutcomes = new List<TestOutcome>();
        }

        public List<TestOutcome> TestsOutcomes { get; }

        public Status Result { get; set; }

        public TimeSpan ExecutionTime { get; set; }

        public int TotalTests => TestsOutcomes.Count;

        public int PassedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Passed));

        public int FailedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Failed));

        public int SkippedTests => TestsOutcomes.Count(o => o.Result.Equals(Status.Skipped));

        public HashSet<string> Bugs { get; }
    }
}
