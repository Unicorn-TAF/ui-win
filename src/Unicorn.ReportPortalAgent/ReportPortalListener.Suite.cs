using System;
using System.Collections.Generic;
using System.Text;
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

                startSuiteRequest.Tags = new List<string>
                {
                    Environment.MachineName
                };

                if (commonSuitesTags != null)
                {
                    startSuiteRequest.Tags.AddRange(commonSuitesTags);
                }

                var test = 
                    parentId.Equals(Guid.Empty) || !this.suitesFlow.ContainsKey(parentId) ?
                    Bridge.Context.LaunchReporter.StartChildTestReporter(startSuiteRequest) :
                    this.suitesFlow[parentId].StartChildTestReporter(startSuiteRequest);

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

                if (parentId.Equals(Guid.Empty) && this.suitesFlow.ContainsKey(id))
                {
                    var tags = new List<string>
                    {
                        Environment.MachineName
                    };

                    // adding tags to suite
                    if (suite.Tags != null)
                    {
                        tags.AddRange(suite.Tags);
                    }

                    if (commonSuitesTags != null)
                    {
                        tags.AddRange(commonSuitesTags);
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

                    // finishing suite
                    var finishSuiteRequest = new FinishTestItemRequest
                    {
                        EndTime = DateTime.UtcNow,
                        Description = description.ToString(),
                        Tags = tags,
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
    }
}