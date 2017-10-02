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
            Bugs = new string[0];
            FailedTests = 0;
        }

        public void FillWithTestsResults(List<Test> testsList)
        {
            List<string> bugsList = new List<string>();
            foreach (Test test in testsList)
            {
                if (test.Outcome.Result == Result.FAILED)
                {
                    FailedTests++;
                    Result = Result.FAILED;
                }

                foreach(string bug in test.Outcome.Bugs)
                    bugsList.Add(bug);
            }

            Bugs = new string[bugsList.Count];
            bugsList.CopyTo(Bugs);
        }
    }
}
