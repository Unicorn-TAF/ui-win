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
        }

        public void FillWithTestsResults(List<Test> testsList)
        {
            HashSet<string> bugsList = new HashSet<string>();
            foreach (Test test in testsList)
            {
                if (test.Outcome.Result == Result.FAILED)
                    Result = Result.FAILED;

                foreach(string bug in test.Outcome.Bugs)
                    bugsList.Add(bug);
            }

            Bugs = new string[bugsList.Count];
            bugsList.CopyTo(Bugs);


            /*Logger.Instance.Info("============================================");
            foreach (Test test in testsList)
            {
                Logger.Instance.Info("Test:" + test.Description);
                Logger.Instance.Info("Result:" + test.Outcome.Result.ToString());
                Logger.Instance.Info("Exec time:" + test.Outcome.ExecutionTime.ToString());

                if(test.Outcome.Exception != null)
                    Logger.Instance.Info("Exception:" + test.Outcome.Exception.ToString());

                if (test.Outcome.Screenshot != null)
                    Logger.Instance.Info("Screenshot:" + test.Outcome.Screenshot.ToString());

                if (test.Outcome.Bugs != null)
                    Logger.Instance.Info("Bugs:" + string.Join(",", test.Outcome.Bugs));
                Logger.Instance.Info("\n\n\n");
            }
            //TODO:*/
        }
    }
}
