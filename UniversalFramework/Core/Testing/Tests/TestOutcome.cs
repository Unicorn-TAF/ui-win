using System;

namespace Unicorn.Core.Testing.Tests
{
    public class TestOutcome
    {
        /// <summary>
        /// Execution Result.
        /// </summary>
        public Result Result;

        /// <summary>
        /// Test execution time as TimeSpan.
        /// </summary>
        public TimeSpan ExecutionTime;

        /// <summary>
        /// Fail Exception details. Has value only when test has failed.
        /// </summary>
        public Exception Exception;

        /// <summary>
        /// Screenshot.
        /// </summary>
        public string Screenshot;

        /// <summary>
        /// Array of bugs attached to the test. Has values only when the test failed by bug.
        /// </summary>
        public string[] Bugs;


        /// <summary>
        /// Test outcome. Inits with zero execution time and empty bugs array
        /// </summary>
        public TestOutcome()
        {
            ExecutionTime = TimeSpan.FromSeconds(0);
            Bugs = new string[0];
        }


    }
}
