using System;
using System.Collections.Generic;

namespace Unicorn.Core.Testing.Tests
{
    public class SuiteOutcome
    {
        public SuiteOutcome()
        {
            this.Bugs = new List<string>();
            this.FailedTests = 0;
        }

        public Result Result { get; set; }

        public TimeSpan ExecutionTime { get; set; }

        public int TotalTests { get; set; }

        public int FailedTests { get; set; }

        public List<string> Bugs { get; }

        public void FillWithTestsResults(List<Test> testsList)
        {
            foreach (Test test in testsList)
            {
                if (test.Outcome.Result == Result.FAILED)
                {
                    this.FailedTests++;
                    this.Result = Result.FAILED;
                }

                foreach (string bug in test.Outcome.Bugs)
                {
                    this.Bugs.Add(bug);
                }
            }
        }
    }
}
