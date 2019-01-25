using ReportPortal.Client.Requests;
using ReportPortal.Shared;
////using System.Web.Script.Serialization;

namespace Unicorn.ReportPortalAgent
{
    public class BridgeExtension : IBridgeExtension
    {
        public bool Handled { get; set; }

        public int Order => int.MaxValue;

        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            ////var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            this.Handled = true;
        }
    }
}
