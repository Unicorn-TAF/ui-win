using System;
using System.Collections.Generic;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Unicorn.Taf.Core.Testing.Tests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        protected void AddAttachment(Guid id, LogLevel level, string text, string attachmantName, string mime, byte[] content)
        {
            try
            {
                this.testFlowIds[id].Log(new AddLogItemRequest
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = text,
                    Attach = new Attach(attachmantName, mime, content)
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void AddLog(Guid id, LogLevel level, string text)
        {
            try
            {
                this.testFlowIds[id].Log(new AddLogItemRequest
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = text,
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void AddTestTags(Test test, params string[] tags)
        {
            try
            {
                var id = test.Outcome.Id;
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
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}
