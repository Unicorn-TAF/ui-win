using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing.Tests;

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

        public Status RunStatus => this.SuitesOutcomes
                .Any(o => o.Result.Equals(Status.Failed) || o.Result.Equals(Status.Skipped)) ?
                Status.Failed :
                Status.Passed;
    }
}
