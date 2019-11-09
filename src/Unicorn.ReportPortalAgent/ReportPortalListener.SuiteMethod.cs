using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private readonly Dictionary<SuiteMethodType, TestItemType> _itemTypes =
            new Dictionary<SuiteMethodType, TestItemType>
        {
            { SuiteMethodType.BeforeSuite, TestItemType.BeforeClass },
            { SuiteMethodType.BeforeTest, TestItemType.BeforeMethod },
            { SuiteMethodType.AfterTest, TestItemType.AfterMethod },
            { SuiteMethodType.AfterSuite, TestItemType.AfterClass },
            { SuiteMethodType.Test, TestItemType.Step },
        };

        internal string SkippedTestDefectType { get; set; } = "ND001";

        internal void StartSuiteMethod(SuiteMethod suiteMethod)
        {
            try
            {
                var id = suiteMethod.Outcome.Id;
                var parentId = suiteMethod.Outcome.ParentId;
                var name = suiteMethod.Outcome.Title;

                _currentTest = suiteMethod;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = _itemTypes[suiteMethod.MethodType]
                };

                startTestRequest.Tags = new List<string>();
                startTestRequest.Tags.Add(suiteMethod.Outcome.Author);
                startTestRequest.Tags.Add(Environment.MachineName);

                var testVal = _suitesFlow[parentId].StartChildTestReporter(startTestRequest);
                _testFlowIds[id] = testVal;
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
                var id = suiteMethod.Outcome.Id;
                var result = suiteMethod.Outcome.Result;

                _currentTest = null;

                if (!_testFlowIds.ContainsKey(id))
                {
                    return;
                }

                // adding categories to test
                var tags = new List<string>();
                tags.Add(suiteMethod.Outcome.Author);
                tags.Add(Environment.MachineName);

                if (suiteMethod.MethodType.Equals(SuiteMethodType.Test))
                {
                    tags.AddRange((suiteMethod as Test).Categories);
                }

                // adding description to test
                var description =
                    suiteMethod.Outcome.Result == Taf.Core.Testing.Status.Failed ?
                    suiteMethod.Outcome.Exception.Message :
                    string.Empty;

                // adding failure items
                if (suiteMethod.Outcome.Result == Taf.Core.Testing.Status.Failed)
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
                    Description = description,
                    Tags = tags,
                    Status = _statusMap[result]
                };

                // adding issue to finish test if failed test has a defect
                if (suiteMethod.Outcome.Result == Taf.Core.Testing.Status.Failed && suiteMethod.Outcome.Defect != null)
                {
                    finishTestRequest.Issue = new Issue
                    {
                        Type = suiteMethod.Outcome.Defect.DefectType,
                        Comment = suiteMethod.Outcome.Defect.Comment
                    };
                }

                // finishing test
                _testFlowIds[id].Finish(finishTestRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        internal void SkipSuiteMethod(SuiteMethod suiteMethod)
        {
            try
            {
                var id = suiteMethod.Outcome.Id;
                var parentId = suiteMethod.Outcome.ParentId;
                var name = suiteMethod.Outcome.Title;
                var result = suiteMethod.Outcome.Result;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = _itemTypes[suiteMethod.MethodType]
                };

                startTestRequest.Tags = new List<string>();
                startTestRequest.Tags.Add(suiteMethod.Outcome.Author);
                startTestRequest.Tags.Add(Environment.MachineName);

                if (suiteMethod.MethodType.Equals(SuiteMethodType.Test))
                {
                    startTestRequest.Tags.AddRange((suiteMethod as Test).Categories);
                }

                var testVal = _suitesFlow[parentId].StartChildTestReporter(startTestRequest);
                _testFlowIds[id] = testVal;

                var finishTestRequest = new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = _statusMap[result],
                    Issue = new Issue
                    {
                        Type = SkippedTestDefectType,
                        Comment = "The test is skipped, check if dependent test or before suite failed"
                    }
                };

                // finishing test
                _testFlowIds[id].Finish(finishTestRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}