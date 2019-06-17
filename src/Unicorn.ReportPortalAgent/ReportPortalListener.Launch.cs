using System;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        internal void StartRun()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.ExistingLaunchId))
                {
                    Bridge.Context.LaunchReporter = new LaunchReporter(Bridge.Service);
                    Bridge.Context.LaunchReporter.StartTask = Task.Run(() => { Bridge.Context.LaunchReporter.LaunchId = ExistingLaunchId; });
                    return;
                }

                LaunchMode launchMode;
                if (Config.Launch.IsDebugMode)
                {
                    launchMode = LaunchMode.Debug;
                }
                else
                {
                    launchMode = LaunchMode.Default;
                }

                var startLaunchRequest = new StartLaunchRequest
                {
                    Name = Config.Launch.Name,
                    Description = Config.Launch.Description,
                    StartTime = DateTime.UtcNow,
                    Mode = launchMode,
                    Tags = Config.Launch.Tags
                };

                Bridge.Context.LaunchReporter = new LaunchReporter(Bridge.Service);
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
                if (!string.IsNullOrEmpty(this.ExistingLaunchId))
                {
                    Bridge.Context.LaunchReporter.FinishTask = Task.Run(() =>
                    {
                        Bridge.Context.LaunchReporter.StartTask.Wait();

                        foreach (var testNode in Bridge.Context.LaunchReporter.TestNodes)
                        {
                            testNode.FinishTask.Wait();
                        }
                    });
                    Bridge.Context.LaunchReporter.FinishTask.Wait();
                    return;
                }

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
