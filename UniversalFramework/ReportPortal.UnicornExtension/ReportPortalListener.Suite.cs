using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.UnicornExtension.EventArguments;
using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicorn.Core.Testing.Tests;
using System.IO;
using Unicorn.Core.Reporting;
using System.Text;
using Unicorn.Core.Logging;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        public delegate void SuiteStartedHandler(object sender, TestItemStartedEventArgs e);
        public static event SuiteStartedHandler BeforeSuiteStarted;
        public static event SuiteStartedHandler AfterSuiteStarted;

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
                    if (parentId.Equals(Guid.Empty) || !_suitesFlow.ContainsKey(parentId))
                    {
                        test = Bridge.Context.LaunchReporter.StartNewTestNode(startSuiteRequest);
                    }
                    else
                    {
                        test = _suitesFlow[parentId].StartNewTestNode(startSuiteRequest);
                    }

                    _suitesFlow[id] = test;

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

        public delegate void SuiteFinishedHandler(object sender, TestItemFinishedEventArgs e);
        public static event SuiteFinishedHandler BeforeSuiteFinished;
        public static event SuiteFinishedHandler AfterSuiteFinished;

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
                    if (_suitesFlow.ContainsKey(id))
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
                            try
                            {
                                updateSuiteRequest.Tags.Add(suite.Metadata["Author"]);
                            }
                            catch { }
                        }

                        // adding description to suite
                        var description = suite.Name;
                        if (description != null)
                        {
                            updateSuiteRequest.Description = description;
                        }

                        if (updateSuiteRequest.Description != null || updateSuiteRequest.Tags != null)
                        {
                            _suitesFlow[id].AdditionalTasks.Add(Task.Run(() =>
                            {
                                _suitesFlow[id].StartTask.Wait();
                                Bridge.Service.UpdateTestItem(_suitesFlow[id].TestId, updateSuiteRequest);
                            }));
                        }

                        // finishing suite
                        var finishSuiteRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = _statusMap[result]
                        };
                        
                        var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, _suitesFlow[id]);

                        try
                        {
                            BeforeSuiteFinished?.Invoke(this, eventArg);
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Error("Exception was thrown in 'BeforeSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }

                        _suitesFlow[id].Finish(finishSuiteRequest);

                        try
                        {
                            AfterSuiteFinished?.Invoke(this, new TestItemFinishedEventArgs(Bridge.Service, finishSuiteRequest, _suitesFlow[id]));
                        }
                        catch (Exception exp)
                        {
                            Logger.Instance.Error("Exception was thrown in 'AfterSuiteFinished' subscriber." + Environment.NewLine + exp);
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }


        protected void AddSuiteTags(TestSuite suite, params string[] tags)
        {
            try
            {
                var id = suite.Id;
                if (_suitesFlow.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();
                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.AddRange(tags);

                    _suitesFlow[id].Update(updateTestRequest);
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

                CurrentTest = test;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Step
                };

                var testVal = _suitesFlow[parentId].StartNewTestNode(startTestRequest);

                _testFlowIds[id] = testVal;

                _testFlowNames[fullname] = testVal;

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

                CurrentTest = null;

                if (_testFlowIds.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();

                    // adding categories to test
                    var categories = test.Categories;

                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.Add(test.Author);
                    if (categories != null)
                    {
                        foreach (string category in categories)
                            updateTestRequest.Tags.Add(category);
                    }

                    // adding description to test
                    var description = test.Description;
                    if (description != null)
                        updateTestRequest.Description = description;

                    if (updateTestRequest.Description != null || updateTestRequest.Tags != null)
                        _testFlowIds[id].Update(updateTestRequest);

                    // adding failure items

                    if (test.Outcome.Result == Result.FAILED)
                    {
                        var failureMessage = test.Outcome.Exception.Message;
                        var failureStacktrace = test.Outcome.Exception.StackTrace;

                        if (!string.IsNullOrEmpty(test.Outcome.Screenshot))
                        {
                            byte[] screenshotBytes = File.ReadAllBytes(Path.Combine(Screenshot.SCREENSHOTS_FOLDER, test.Outcome.Screenshot));

                            _testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                                Attach = new Attach(test.Outcome.Screenshot, "image/jpeg", screenshotBytes)
                            });
                        }
                        else
                        {
                            _testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                            });
                        }

                        _testFlowIds[id].Log(new AddLogItemRequest
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
                            Status = _statusMap[result]
                        };
                    }
                    else
                    {
                        finishTestRequest = new FinishTestItemRequest
                        {
                            EndTime = DateTime.UtcNow,
                            Status = _statusMap[result]
                        };
                    }

                    _testFlowIds[id].Finish(finishTestRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}