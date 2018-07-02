using System;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        private SuiteMethod currentTest = null;

        protected void TestOutput(string info)
        {
            try
            {
                var fullTestName = this.currentTest.FullName;
                var message = info;

                if (this.testFlowNames.ContainsKey(fullTestName))
                {
                    ////var serializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
                    ////AddLogItemRequest logRequest = null;
                    ////try
                    ////{
                    ////    logRequest = serializer.Deserialize<AddLogItemRequest>(message);
                    ////}
                    ////catch (Exception ex)
                    ////{
                    ////    Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + ex);
                    ////}

                    ////if (logRequest != null)
                    ////    _testFlowNames[fullTestName].Log(logRequest);
                    ////else
                    this.testFlowNames[fullTestName].Log(new AddLogItemRequest { Level = Client.Models.LogLevel.Info, Time = DateTime.UtcNow, Text = message });
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Error, "ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}
