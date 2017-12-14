using ReportPortal.Client.Requests;
using ReportPortal.Shared;
using System.Web.Script.Serialization;

namespace ReportPortal.UnicornExtension
{
    public class BridgeExtension : IBridgeExtension
    {
        public bool Handled { get; set; }

        public int Order => int.MaxValue;

        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            ////NUnit.Framework.TestContext.Progress.WriteLine(serializer.Serialize(logRequest));
            Handled = true;
        }
    }
}
