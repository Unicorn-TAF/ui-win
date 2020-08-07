using System;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private void AddAttachment(Guid id, LogLevel level, string text, string attachmentName, string mime, byte[] content)
        {
            try
            {
                _testFlowIds[id].Log(new AddLogItemRequest
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = text,
                    Attach = new Attach(attachmentName, mime, content)
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        private void AddLog(Guid id, LogLevel level, string text)
        {
            try
            {
                _testFlowIds[id].Log(new AddLogItemRequest
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
