using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        internal void StartSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Outcome.Id;
                var parentId = Guid.Empty;
                var name = suite.Outcome.Name;

                if (!string.IsNullOrEmpty(suite.Outcome.DataSetName))
                {
                    name += "[" + suite.Outcome.DataSetName + "]";
                }

                var startSuiteRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = TestItemType.Suite
                };

                startSuiteRequest.Tags = new List<string>();
                startSuiteRequest.Tags.Add(Environment.MachineName);

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
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        internal void FinishSuite(TestSuite suite)
        {
            try
            {
                var id = suite.Outcome.Id;
                var result = suite.Outcome.Result;
                var parentId = Guid.Empty;

                // at the end of execution nunit raises 2 the same events, we need only that which has 'parentId' xml tag
                if (parentId.Equals(Guid.Empty) && this.suitesFlow.ContainsKey(id))
                {
                    var updateSuiteRequest = new UpdateTestItemRequest();

                    updateSuiteRequest.Tags = new List<string>();
                    updateSuiteRequest.Tags.Add(Environment.MachineName);

                    // adding tags to suite
                    if (suite.Tags != null)
                    {
                        updateSuiteRequest.Tags.AddRange(suite.Tags);
                    }

                    // adding description to suite
                    var description = new StringBuilder();

                    foreach (var key in suite.Metadata.Keys)
                    {
                        var value = suite.Metadata[key];
                        var appendString = 
                            value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) ?
                            $"[{key}]({value})" : 
                            $"{key}: {value}";
                        
                        description.AppendLine(appendString);
                    }

                    if (description.Length != 0)
                    {
                        updateSuiteRequest.Description = description.ToString();
                    }

                    if (updateSuiteRequest.Description != null || updateSuiteRequest.Tags != null)
                    {
                        this.suitesFlow[id].AdditionalTasks.Add(Task.Run(() =>
                        {
                            this.suitesFlow[id].StartTask.Wait();
                            Bridge.Service.UpdateTestItemAsync(this.suitesFlow[id].TestId, updateSuiteRequest);
                        }));
                    }

                    // finishing suite
                    var finishSuiteRequest = new FinishTestItemRequest
                    {
                        EndTime = DateTime.UtcNow,
                        Status = result.Equals(Taf.Core.Testing.Status.Skipped) ? ReportPortal.Client.Models.Status.Failed : statusMap[result]
                    };
                        
                    this.suitesFlow[id].Finish(finishSuiteRequest);
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
                var id = suite.Outcome.Id;
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
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}