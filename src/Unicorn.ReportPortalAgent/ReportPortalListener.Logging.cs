using System;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        internal void AddAttachment(Guid id, LogLevel level, string text, string attachmantName, string mime, byte[] content)
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

        internal void AddLog(Guid id, LogLevel level, string text)
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
    }
}
