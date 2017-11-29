using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;
using System.Web.Script.Serialization;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {

        TestSuiteMethodBase CurrentTest = null;


        protected void TestOutput(string info)
        {
            try
            {
                var fullTestName = CurrentTest.FullTestName;
                var message = info;

                if (_testFlowNames.ContainsKey(fullTestName))
                {
                    //var serializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
                    //AddLogItemRequest logRequest = null;
                    //try
                    //{
                    //    logRequest = serializer.Deserialize<AddLogItemRequest>(message);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + ex);
                    //}
                    
                    //if (logRequest != null)
                    //    _testFlowNames[fullTestName].Log(logRequest);
                    //else
                        _testFlowNames[fullTestName].Log(new AddLogItemRequest { Level = LogLevel.Info, Time = DateTime.UtcNow, Text = message});
                    
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

    }
}
