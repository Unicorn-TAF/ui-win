using System;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using ReportPortal.Shared.Reporter;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        internal void StartRun()
        {
            try
            {
                LaunchMode launchMode = 
                    Config.Launch.IsDebugMode ? LaunchMode.Debug : LaunchMode.Default;

                var startLaunchRequest = new StartLaunchRequest
                {
                    Name = Config.Launch.Name,
                    Description = Config.Launch.Description,
                    StartTime = DateTime.UtcNow,
                    Mode = launchMode,
                    Tags = Config.Launch.Tags
                };

                Bridge.Context.LaunchReporter =
                    string.IsNullOrEmpty(ExistingLaunchId) ?
                    new LaunchReporter(Bridge.Service) :
                    new LaunchReporter(Bridge.Service, ExistingLaunchId);

                Bridge.Context.LaunchReporter.Start(startLaunchRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        internal void FinishRun()
        {
            try
            {
                var finishLaunchRequest = new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow,
                };

                Bridge.Context.LaunchReporter.Finish(finishLaunchRequest);
                Bridge.Context.LaunchReporter.FinishTask.Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}
