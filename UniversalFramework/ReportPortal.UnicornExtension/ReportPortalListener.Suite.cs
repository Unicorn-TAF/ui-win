using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using ReportPortal.UnicornExtension.EventArguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
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
                    Logger.Instance.Error("Exception was thrown in 'BeforeSuiteStarted' subscriber." + Environment.NewLine + exp);
                }

                if (!beforeSuiteEventArg.Canceled)
                {
                    TestReporter test;
                    if (parentId.Equals(Guid.Empty) || !suitesFlow.ContainsKey(parentId))
                    {
                        test = Bridge.Context.LaunchReporter.StartNewTestNode(startSuiteRequest);
                    }
                    else
                    {
                        test = suitesFlow[parentId].StartNewTestNode(startSuiteRequest);
                    }

                    suitesFlow[id] = test;

                    try
                    {
                        AfterSuiteStarted?.Invoke(this, new TestItemStartedEventArgs(Bridge.Service, startSuiteRequest, test));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'AfterSuiteStarted' subscriber." + Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
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
                    if (suitesFlow.ContainsKey(id))
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
                        var description = suite.Name;
                        if (description != null)
                        {
                            updateSuiteRequest.Description = description;
                        }

                        if (updateSuiteRequest.Description != null || updateSuiteRequest.Tags != null)
                        {
                            suitesFlow[id].AdditionalTasks.Add(Task.Run(() =>
                            {
                                suitesFlow[id].StartTask.Wait();
                                Bridge.Service.UpdateTestItem(suitesFlow[id].TestId, updateSuiteRequest);
                            }));
                        }

                        // finishing suite
                        var finishSuiteRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = statusMap[result]
                        };
                        
                        var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, suitesFlow[id]);

                        try
                        {
                            BeforeSuiteFinished?.Invoke(this, eventArg);
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Error("Exception was thrown in 'BeforeSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }

                        suitesFlow[id].Finish(finishSuiteRequest);

                        try
                        {
                            AfterSuiteFinished?.Invoke(this, new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, suitesFlow[id]));
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Error("Exception was thrown in 'AfterSuiteFinished' subscriber." + Environment.NewLine + exp);
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
                if (suitesFlow.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();
                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.AddRange(tags);

                    suitesFlow[id].Update(updateTestRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void StartSuiteMethod(TestSuiteMethod test)
        {
            try
            {
                var id = test.Id;
                var parentId = test.ParentId;
                var name = test.Description;
                var fullname = test.FullTestName;

                this.currentTest = test;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Step
                };

                var testVal = suitesFlow[parentId].StartNewTestNode(startTestRequest);

                testFlowIds[id] = testVal;

                testFlowNames[fullname] = testVal;
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void FinishSuiteMethod(TestSuiteMethod test)
        {
            try
            {
                var id = test.Id;
                var result = test.Outcome.Result;
                var parentId = test.ParentId;

                this.currentTest = null;

                if (testFlowIds.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();

                    // adding categories to test
                    var categories = test.Categories;

                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.Add(test.Author);

                    if (categories != null)
                    {
                        foreach (string category in categories)
                        {
                            updateTestRequest.Tags.Add(category);
                        }
                    }

                    // adding description to test
                    var description = test.Description;

                    if (description != null)
                    {
                        updateTestRequest.Description = description;
                    }

                    if (updateTestRequest.Description != null || updateTestRequest.Tags != null)
                    {
                        testFlowIds[id].Update(updateTestRequest);
                    }

                    // adding failure items
                    if (test.Outcome.Result == Result.FAILED)
                    {
                        var failureMessage = test.Outcome.Exception.Message;
                        var failureStacktrace = test.Outcome.Exception.StackTrace;

                        if (!string.IsNullOrEmpty(test.Outcome.Screenshot))
                        {
                            byte[] screenshotBytes = File.ReadAllBytes(Path.Combine(Screenshot.ScreenshotsFolder, test.Outcome.Screenshot));

                            testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                                Attach = new Attach(test.Outcome.Screenshot, "image/jpeg", screenshotBytes)
                            });
                        }
                        else
                        {
                            testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                            });
                        }

                        testFlowIds[id].Log(new AddLogItemRequest
                        {
                            Level = LogLevel.Error,
                            Time = DateTime.UtcNow,
                            Text = "Attachment: Log file",
                            Attach = new Attach(test.Outcome.Screenshot, "text/plain", Encoding.ASCII.GetBytes(Test.CurrentOutput.ToString()))
                        });
                    }

                    FinishTestItemRequest finishTestRequest = null;

                    // finishing test
                    if (test.Outcome.Result == Result.FAILED && !string.IsNullOrEmpty(test.Outcome.OpenBugString))
                    {
                        string iss = test.Outcome.OpenBugString;
                        Issue issue = new Issue();
                        issue.Type = IssueType.ProductionBug;
                        issue.Comment = test.Outcome.OpenBugString;

                        finishTestRequest = new FinishTestItemRequest
                        {
                            Issue = issue,
                            EndTime = DateTime.UtcNow,
                            Status = statusMap[result]
                        };
                    }
                    else
                    {
                        finishTestRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = statusMap[result]
                        };
                    }

                    testFlowIds[id].Finish(finishTestRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}