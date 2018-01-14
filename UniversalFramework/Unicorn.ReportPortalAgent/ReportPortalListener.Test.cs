using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using ReportPortal.UnicornExtension.EventArguments;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        public delegate void TestStartedHandler(object sender, TestItemStartedEventArgs e);

        public delegate void TestFinishedHandler(object sender, TestItemFinishedEventArgs e);

        public static event TestStartedHandler BeforeTestStarted;

        public static event TestStartedHandler AfterTestStarted;

        public static event TestFinishedHandler BeforeTestFinished;

        public static event TestFinishedHandler AfterTestFinished;

        protected void StartTest(Test test)
        {
            try
            {
                var id = test.Id;
                var parentId = test.ParentId;
                var name = test.Description;
                var fullname = test.FullName;

                this.currentTest = test;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Test
                };

                var beforeTestEventArg = new TestItemStartedEventArgs(Bridge.Service, startTestRequest);
                try
                {
                    BeforeTestStarted?.Invoke(this, beforeTestEventArg);
                }
                catch (Exception exp)
                {
                    Logger.Instance.Error("Exception was thrown in 'BeforeTestStarted' subscriber." + Environment.NewLine +
                                      exp);
                }

                if (!beforeTestEventArg.Canceled)
                {
                    var testVal = this.suitesFlow[parentId].StartNewTestNode(startTestRequest);

                    this.testFlowIds[id] = testVal;

                    this.testFlowNames[fullname] = testVal;

                    try
                    {
                        AfterTestStarted?.Invoke(this, new TestItemStartedEventArgs(Bridge.Service, startTestRequest, testVal));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'AfterTestStarted' subscriber." + Environment.NewLine +
                                          exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void FinishTest(Test test)
        {
            try
            {
                var id = test.Id;
                var result = test.Outcome.Result;
                var parentId = test.ParentId;

                this.currentTest = null;

                if (this.testFlowIds.ContainsKey(id))
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
                        this.testFlowIds[id].Update(updateTestRequest);
                    }

                    // adding failure items
                    if (test.Outcome.Result == Result.FAILED)
                    {
                        var failureMessage = test.Outcome.Exception.Message;
                        var failureStacktrace = test.Outcome.Exception.StackTrace;

                        if (!string.IsNullOrEmpty(test.Outcome.Screenshot))
                        {
                            byte[] screenshotBytes = File.ReadAllBytes(Path.Combine(Screenshot.ScreenshotsFolder, test.Outcome.Screenshot));

                            this.testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                                Attach = new Attach(test.Outcome.Screenshot, "image/jpeg", screenshotBytes)
                            });
                        }
                        else
                        {
                            this.testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                            });
                        }

                        this.testFlowIds[id].Log(new AddLogItemRequest
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

                    var eventArg = new TestItemFinishedEventArgs(Bridge.Service, finishTestRequest, this.testFlowIds[id]);

                    try
                    {
                        BeforeTestFinished?.Invoke(this, eventArg);
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'BeforeTestFinished' subscriber." +
                                          Environment.NewLine + exp);
                    }

                    this.testFlowIds[id].Finish(finishTestRequest);

                    try
                    {
                        AfterTestFinished?.Invoke(
                            this,
                            new TestItemFinishedEventArgs(Bridge.Service, finishTestRequest, this.testFlowIds[id]));
                    }
                    catch (Exception exp)
                    {
                        Logger.Instance.Error("Exception was thrown in 'AfterTestFinished' subscriber." +
                                          Environment.NewLine + exp);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void AddAttachment(Test test, string name, string mime, byte[] content)
        {
            try
            {
                var id = test.Id;
                if (this.testFlowIds.ContainsKey(id))
                {
                    this.testFlowIds[id].Log(new AddLogItemRequest
                    {
                        Level = LogLevel.None,
                        Time = DateTime.UtcNow,
                        Text = "Attachment: " + name,
                        Attach = new Attach(test.Outcome.Screenshot, mime, content)
                    });
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void AddTestTags(Test test, params string[] tags)
        {
            try
            {
                var id = test.Id;
                if (this.testFlowIds.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();
                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.AddRange(tags);

                    this.testFlowIds[id].Update(updateTestRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}
