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
        private readonly Dictionary<SuiteMethodType, TestItemType> itemTypes =
            new Dictionary<SuiteMethodType, TestItemType>
        {
            { SuiteMethodType.BeforeSuite, TestItemType.BeforeClass },
            { SuiteMethodType.BeforeTest, TestItemType.BeforeMethod },
            { SuiteMethodType.AfterTest, TestItemType.AfterMethod },
            { SuiteMethodType.AfterSuite, TestItemType.AfterClass },
            { SuiteMethodType.Test, TestItemType.Step },
        };

        internal void StartSuiteMethod(SuiteMethod test)
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
                    Type = itemTypes[test.MethodType]
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

        internal void FinishSuiteMethod(SuiteMethod suiteMethod)
        {
            try
            {
                var id = suiteMethod.Id;
                var result = suiteMethod.Outcome.Result;

                this.currentTest = null;

                if (!this.testFlowIds.ContainsKey(id))
                {
                    return;
                }

                var updateTestRequest = new UpdateTestItemRequest();

                // adding categories to test
                updateTestRequest.Tags = new List<string>();
                updateTestRequest.Tags.Add(suiteMethod.Author);

                if (suiteMethod.MethodType.Equals(SuiteMethodType.Test))
                {
                    (suiteMethod as Test).Categories.ForEach(c => updateTestRequest.Tags.Add(c));
                }

                // adding description to test
                updateTestRequest.Description = suiteMethod.Description;

                this.testFlowIds[id].Update(updateTestRequest);

                // adding failure items
                if (suiteMethod.Outcome.Result == Core.Testing.Tests.Status.Failed)
                {
                    var text = suiteMethod.Outcome.Exception.Message + Environment.NewLine + suiteMethod.Outcome.Exception.StackTrace;

                    if (!string.IsNullOrEmpty(suiteMethod.Outcome.Screenshot))
                    {
                        byte[] screenshotBytes = File.ReadAllBytes(suiteMethod.Outcome.Screenshot);
                        AddAttachment(id, LogLevel.Error, text, "Fail screenshot", "image/png", screenshotBytes);
                    }
                    else
                    {
                        AddLog(id, LogLevel.Error, text);
                    }

                    if (!string.IsNullOrEmpty(suiteMethod.Outcome.Output))
                    {
                        byte[] outputBytes = Encoding.ASCII.GetBytes(suiteMethod.Outcome.Output);
                        AddAttachment(id, LogLevel.Error, string.Empty, "Execution log", "text/plain", outputBytes);
                    }
                }

                var finishTestRequest = new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = statusMap[result]
                };

                // finishing test
                if (suiteMethod.Outcome.Result == Core.Testing.Tests.Status.Failed)
                {
                    var type = suiteMethod.Outcome.Defect == null
                        ? Core.Testing.Defect.ToInvestigate
                        : suiteMethod.Outcome.Defect.DefectType;

                    var comment = suiteMethod.Outcome.Defect == null
                        ? string.Empty
                        : suiteMethod.Outcome.Defect.Comment;

                    finishTestRequest.Issue = new Issue
                    {
                        Type = type,
                        Comment = comment
                    };
                }

                this.testFlowIds[id].Finish(finishTestRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}