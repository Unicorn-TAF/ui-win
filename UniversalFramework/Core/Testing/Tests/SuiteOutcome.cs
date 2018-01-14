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
    }
}
