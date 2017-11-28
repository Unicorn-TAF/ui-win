using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.UnicornExtension.EventArguments;
using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using ReportPortal.Client.Filtering;
using System.Threading.Tasks;
using Unicorn.Core.Logging;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        public delegate void RunStartedHandler(object sender, RunStartedEventArgs e);
        public static event RunStartedHandler BeforeRunStarted;
        public static event RunStartedHandler AfterRunStarted;

        protected void StartRun()
        {
            try
            {
                if (!string.IsNullOrEmpty(ExistingLaunchId))
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

                var eventArg = new RunStartedEventArgs(Bridge.Service, startLaunchRequest);

                try
                {
                    BeforeRunStarted?.Invoke(this, eventArg);
                }
                catch (Exception exp)
                {
                    Logger.Instance.Error("Exception was thrown in 'BeforeRunStarted' subscriber." + Environment.NewLine + exp);
                }

                if (!eventArg.Canceled)
                {
                    Bridge.Context.LaunchReporter = new LaunchReporter(Bridge.Service);
                    Bridge.Context.LaunchReporter.Start(eventArg.Launch);

                    try
                    {
                        AfterRunStarted?.Invoke(this, new RunStartedEventArgs(Bridge.Service, startLaunchRequest, Bridge.Context.LaunchReporter));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'AfterRunStarted' subscriber." + Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        public delegate void RunFinishedHandler(object sender, RunFinishedEventArgs e);
        public static event RunFinishedHandler BeforeRunFinished;
        public static event RunFinishedHandler AfterRunFinished;

        protected void FinishRun()
        {
            try
            {
                if (!string.IsNullOrEmpty(ExistingLaunchId))
                {
                    Bridge.Context.LaunchReporter.FinishTask = Task.Run(() =>
                    {
                        Bridge.Context.LaunchReporter.StartTask.Wait(); Bridge.Context.LaunchReporter.TestNodes.ForEach(tn => tn.FinishTask.Wait());
                    });
                    Bridge.Context.LaunchReporter.FinishTask.Wait();
                    return;
                }

                var finishLaunchRequest = new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow,
                    
                };

                var eventArg = new RunFinishedEventArgs(Bridge.Service, finishLaunchRequest, Bridge.Context.LaunchReporter);
                try
                {
                    BeforeRunFinished?.Invoke(this, eventArg);
                }
                catch (Exception exp)
                {
                    Logger.Instance.Error("Exception was thrown in 'BeforeRunFinished' subscriber." + Environment.NewLine + exp);
                }

                if (!eventArg.Canceled)
                {
                    Bridge.Context.LaunchReporter.Finish(finishLaunchRequest);
                    Bridge.Context.LaunchReporter.FinishTask.Wait();

                    try
                    {
                        AfterRunFinished?.Invoke(this, new RunFinishedEventArgs(Bridge.Service, finishLaunchRequest, Bridge.Context.LaunchReporter));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'AfterRunFinished' subscriber." + Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }


        protected void MergeRuns(string descriptionSearchString)
        {
            try
            {
                LaunchesContainer container = GetLaunchesByDescriptionFilter(descriptionSearchString);

                if (container.Launches.Count > 1)
                {
                    MergeLaunchesRequest request = new MergeLaunchesRequest();
                    request.Launches = new List<string>();
                    request.Mode = LaunchMode.Default;
                    request.MergeType = "BASIC";
                    request.Description = container.Launches[0].Description;
                    request.Tags = container.Launches[0].Tags;
                    request.StartTime = container.Launches[0].StartTime.Value;
                    request.EndTime = container.Launches[container.Launches.Count - 1].EndTime.Value;
                    request.Name = container.Launches[0].Name;

                    foreach (Launch launch in container.Launches)
                        if(launch.EndTime != null)
                            request.Launches.Add(launch.Id);

                    Bridge.Service.MergeLaunches(request);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Error merging launches: " + ex.ToString());
            }
        }

        protected string GetLaunchId(string descriptionSearchString)
        {
            try
            {
                LaunchesContainer container = GetLaunchesByDescriptionFilter(descriptionSearchString);

                if (container.Launches.Count > 1)
                    return container.Launches[0].Id;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Error getting existing launch id: " + ex.ToString());
            }
            return null;
        }


        private LaunchesContainer GetLaunchesByDescriptionFilter(string descriptionSearchString)
        {
            FilterOption filteringOptions = new FilterOption();
            filteringOptions.Filters = new List<Filter>();
            Filter filter = new Filter(FilterOperation.Contains, "description", descriptionSearchString);
            filteringOptions.Filters.Add(filter);

            LaunchesContainer container = Bridge.Service.GetLaunches(filteringOptions);
            return container;
        }
    }
}
