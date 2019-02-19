using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Unicorn.Core.Testing.Tests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        internal void StartSuiteMethod(SuiteMethod test)
        {
            try
            {
                var id = test.Id;
                var parentId = test.ParentId;
                var name = test.Description;
                var fullname = test.FullName;

                this.currentTest = test;

                TestItemType itemType = TestItemType.None;

                switch (test.MethodType)
                {
                    case SuiteMethodType.BeforeSuite:
                        itemType = TestItemType.BeforeClass;
                        break;
                    case SuiteMethodType.BeforeTest:
                        itemType = TestItemType.BeforeMethod;
                        break;
                    case SuiteMethodType.AfterTest:
                        itemType = TestItemType.AfterMethod;
                        break;
                    case SuiteMethodType.AfterSuite:
                        itemType = TestItemType.AfterClass;
                        break;
                }

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = itemType
                };

                var testVal = this.suitesFlow[parentId].StartNewTestNode(startTestRequest);

                this.testFlowIds[id] = testVal;

                this.testFlowNames[fullname] = testVal;
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        internal void FinishSuiteMethod(SuiteMethod test)
        {
            try
            {
                var id = test.Id;
                var result = test.Outcome.Result;
                ////var parentId = test.ParentId;

                this.currentTest = null;

                if (this.testFlowIds.ContainsKey(id))
                {
                    var updateTestRequest = new UpdateTestItemRequest();

                    // adding categories to test
                    updateTestRequest.Tags = new List<string>();
                    updateTestRequest.Tags.Add(test.Author);

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
                    if (test.Outcome.Result == Core.Testing.Tests.Status.Failed)
                    {
                        var failureMessage = test.Outcome.Exception.Message;
                        var failureStacktrace = test.Outcome.Exception.StackTrace;

                        if (!string.IsNullOrEmpty(test.Outcome.Screenshot))
                        {
                            byte[] screenshotBytes = File.ReadAllBytes(test.Outcome.Screenshot);

                            this.testFlowIds[id].Log(new AddLogItemRequest
                            {
                                Level = LogLevel.Error,
                                Time = DateTime.UtcNow,
                                Text = failureMessage + Environment.NewLine + failureStacktrace,
                                Attach = new Attach("Fail screenshot", "image/png", screenshotBytes)
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
                            Attach = new Attach("Execution log", "text/plain", Encoding.ASCII.GetBytes(Test.TestOutput.ToString()))
                        });
                    }

                    FinishTestItemRequest finishTestRequest = null;

                    // finishing test
                    if (test.Outcome.Result == Core.Testing.Tests.Status.Failed && !string.IsNullOrEmpty(test.Outcome.OpenBugString))
                    {
                        Issue issue = new Issue();
                        issue.Type = "Product Bug";
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

                    this.testFlowIds[id].Finish(finishTestRequest);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}