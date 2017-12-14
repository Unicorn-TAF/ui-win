using System;
using System.Collections.Generic;

namespace Unicorn.Core.Testing.Tests
{
    public class SuiteOutcome
    {
        public Result Result;

        public TimeSpan ExecutionTime;

        public int TotalTests;

        public int FailedTests;

        public string[] Bugs;

        public SuiteOutcome()
        {
            this.Bugs = new string[0];
            this.FailedTests = 0;
        }

        public void FillWithTestsResults(List<Test> testsList)
        {
            List<string> bugsList = new List<string>(this.Bugs);

            foreach (Test test in testsList)
            {
                if (test.Outcome.Result == Result.FAILED)
                {
                    this.FailedTests++;
                    this.Result = Result.FAILED;
                }

                foreach (string bug in test.Outcome.Bugs)
                {
                    bugsList.Add(bug);
                }
            }

            this.Bugs = new string[bugsList.Count];
            bugsList.CopyTo(this.Bugs);
        }
    }
}
