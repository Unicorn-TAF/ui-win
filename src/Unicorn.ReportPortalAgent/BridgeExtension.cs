using System.Web.Script.Serialization;
using ReportPortal.Client.Requests;
using ReportPortal.Shared;

namespace Unicorn.ReportPortalAgent
{
    public class BridgeExtension : IBridgeExtension
    {
        public bool Handled { get; set; }

        public int Order => int.MaxValue;

        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            ////NUnit.Framework.TestContext.Progress.WriteLine(serializer.Serialize(logRequest));
            this.Handled = true;
        }
    }
}
