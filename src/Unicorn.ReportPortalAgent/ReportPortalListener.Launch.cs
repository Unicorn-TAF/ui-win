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

        ////protected void MergeRuns(string descriptionSearchString)
        ////{
        ////    try
        ////    {
        ////        LaunchesContainer container = GetLaunchesByDescriptionFilter(descriptionSearchString);

        ////        if (container.Launches.Count > 1)
        ////        {
        ////            MergeLaunchesRequest request = new MergeLaunchesRequest();
        ////            request.Launches = new List<string>();
        ////            request.Mode = LaunchMode.Default;
        ////            request.MergeType = "BASIC";
        ////            request.Description = container.Launches[0].Description;
        ////            request.Tags = container.Launches[0].Tags;
        ////            request.StartTime = container.Launches[0].StartTime.Value;
        ////            request.EndTime = container.Launches[container.Launches.Count - 1].EndTime.Value;
        ////            request.Name = container.Launches[0].Name;

        ////            foreach (Launch launch in container.Launches)
        ////            {
        ////                if (launch.EndTime != null)
        ////                {
        ////                    request.Launches.Add(launch.Id);
        ////                }
        ////            }
                        
        ////            Bridge.Service.MergeLaunches(request);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Console.WriteLine("Error merging launches: " + ex);
        ////    }
        ////}

        ////protected string GetLaunchId(string descriptionSearchString)
        ////{
        ////    try
        ////    {
        ////        LaunchesContainer container = GetLaunchesByDescriptionFilter(descriptionSearchString);

        ////        if (container.Launches.Count > 1)
        ////        {
        ////            return container.Launches[0].Id;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Console.WriteLine("Error getting existing launch id: " + ex);
        ////    }

        ////    return null;
        ////}

        ////private LaunchesContainer GetLaunchesByDescriptionFilter(string descriptionSearchString)
        ////{
        ////    FilterOption filteringOptions = new FilterOption();
        ////    filteringOptions.Filters = new List<Filter>();
        ////    Filter filter = new Filter(FilterOperation.Contains, "description", descriptionSearchString);
        ////    filteringOptions.Filters.Add(filter);

        ////    LaunchesContainer container = Bridge.Service.GetLaunches(filteringOptions);
        ////    return container;
        ////}
    }
}
