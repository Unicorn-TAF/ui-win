using System;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;

namespace Unicorn.ReportPortalAgent.EventArguments
{
    public class RunFinishedEventArgs : EventArgs
    {
        public RunFinishedEventArgs(Service service, FinishLaunchRequest request, LaunchReporter launchReporter)
        {
            this.Service = service;
            this.Launch = request;
            this.LaunchReporter = launchReporter;
        }

        public Service Service { get; }

        public FinishLaunchRequest Launch { get; }

        public LaunchReporter LaunchReporter { get; }

        public bool Canceled { get; set; }
    }
}
