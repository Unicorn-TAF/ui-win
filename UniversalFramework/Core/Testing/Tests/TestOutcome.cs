using System;

namespace Unicorn.Core.Testing.Tests
{
    public class TestOutcome
    {
        public Result Result;
        public TimeSpan ExecutionTime;
        public Exception Exception;
        public string Screenshot;
        public string[] Bugs;

        public TestOutcome()
        {
            Bugs = new string[0];
        }


    }
}
