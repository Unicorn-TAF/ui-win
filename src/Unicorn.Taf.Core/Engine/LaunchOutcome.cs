using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Engine
{
    [Serializable]
    public class LaunchOutcome
    {
        public LaunchOutcome()
        {
            this.SuitesOutcomes = new List<SuiteOutcome>();
        }

        public List<SuiteOutcome> SuitesOutcomes { get; }

        public bool RunInitialized { get; set; } = true;

        public Status RunStatus => this.SuitesOutcomes
                .Any(o => o.Result.Equals(Status.Failed) || o.Result.Equals(Status.Skipped)) || !this.RunInitialized ?
                Status.Failed :
                Status.Passed;

        public Exception RunnerException { get; set; } = null;
    }
}
