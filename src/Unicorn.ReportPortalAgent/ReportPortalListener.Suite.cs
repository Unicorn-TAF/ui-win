using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.ReportPortalAgent.EventArguments;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        public delegate void SuiteStartedHandler(object sender, TestItemStartedEventArgs e);

        public delegate void SuiteFinishedHandler(object sender, TestItemFinishedEventArgs e);

        public static event SuiteStartedHandler BeforeSuiteStarted;

        public static event SuiteStartedHandler AfterSuiteStarted;

        public static event SuiteFinishedHandler BeforeSuiteFinished;

        public static event SuiteFinishedHandler AfterSuiteFinished;

        protected void StartSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Id;
                var parentId = Guid.Empty;
                var name = suite.Name;

                var startSuiteRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Suite
                };

                var beforeSuiteEventArg = new TestItemStartedEventArgs(Bridge.Service, startSuiteRequest);
                try
                {
                    BeforeSuiteStarted?.Invoke(this, beforeSuiteEventArg);
                }
                catch (Exception exp)
                {
                    Logger.Instance.Log(Core.Logging.LogLevel.Error, "Exception was thrown in 'BeforeSuiteStarted' subscriber." + Environment.NewLine + exp);
                }

                if (!beforeSuiteEventArg.Canceled)
                {
                    TestReporter test;
                    if (parentId.Equals(Guid.Empty) || !this.suitesFlow.ContainsKey(parentId))
                    {
                        test = Bridge.Context.LaunchReporter.StartNewTestNode(startSuiteRequest);
                    }
                    else
                    {
                        test = this.suitesFlow[parentId].StartNewTestNode(startSuiteRequest);
                    }

                    this.suitesFlow[id] = test;

                    try
                    {
                        AfterSuiteStarted?.Invoke(this, new TestItemStartedEventArgs(Bridge.Service, startSuiteRequest, test));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Log(Core.Logging.LogLevel.Error, "Exception was thrown in 'AfterSuiteStarted' subscriber." + Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(Core.Logging.LogLevel.Error, "ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void FinishSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Id;
                var result = suite.Outcome.Result;
                var parentId = Guid.Empty;

                // at the end of execution nunit raises 2 the same events, we need only that which has 'parentId' xml tag
                if (parentId.Equals(Guid.Empty))
                {
                    if (this.suitesFlow.ContainsKey(id))
                    {
                        var updateSuiteRequest = new UpdateTestItemRequest();

                        // adding categories to suite
                        var categories = suite.Features;
                        if (categories != null)
                        {
                            updateSuiteRequest.Tags = new List<string>();

                            foreach (string category in categories)
                            {
                                updateSuiteRequest.Tags.Add(category);
                            }
                        }

                        // adding description to suite
                        var description = string.Empty;

                        foreach (var key in suite.Metadata.Keys)
                        {
                            description += $"{key}: {suite.Metadata[key]}\n";
                        }

                        if (!string.IsNullOrEmpty(description.Trim()))
                        {
                            updateSuiteRequest.Description = description;
                        }

                        if (updateSuiteRequest.Description != null || updateSuiteRequest.Tags != null)
                        {
                            this.suitesFlow[id].AdditionalTasks.Add(Task.Run(() =>
                            {
                                this.suitesFlow[id].StartTask.Wait();
                                Bridge.Service.UpdateTestItem(this.suitesFlow[id].TestId, updateSuiteRequest);
                            }));
                        }

                        // finishing suite
                        var finishSuiteRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = statusMap[result]
                        };
                        
                        var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, this.suitesFlow[id]);

                        try
                        {
                            BeforeSuiteFinished?.Invoke(this, eventArg);
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Log(Core.Logging.LogLevel.Error, "Exception was thrown in 'BeforeSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }

                        this.suitesFlow[id].Finish(finishSuiteRequest);

                        try
                        {
                            AfterSuiteFinished?.Invoke(this, new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, this.suitesFlow[id]));
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Log(Core.Logging.LogLevel.Error, "Exception was thrown in 'AfterSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void AddSuiteTags(TestSuite suite, params string[] tags)
        {
            try
            {
                var id = suite.Id;
                if (this.suitesFlow.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();
                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.AddRange(tags);

                    this.suitesFlow[id].Update(updateTestRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(Core.Logging.LogLevel.Error, "ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}