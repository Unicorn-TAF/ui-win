using System;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;

namespace ReportPortal.UnicornExtension.EventArguments
{
    public class RunStartedEventArgs : EventArgs
    {
        public RunStartedEventArgs(Service service, StartLaunchRequest request)
        {
            this.Service = service;
            this.Launch = request;
        }

        public RunStartedEventArgs(Service service, StartLaunchRequest request, LaunchReporter launchReporter) : this(service, request)
        {
            LaunchReporter = launchReporter;
        }

        public Service Service { get; }

        public StartLaunchRequest Launch { get; }

        public LaunchReporter LaunchReporter { get; }

        public bool Canceled { get; set; }
    }
}
