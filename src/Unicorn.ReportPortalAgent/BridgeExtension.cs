using ReportPortal.Client.Requests;
using ReportPortal.Shared;
////using System.Web.Script.Serialization;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// RP bridge extension.
    /// </summary>
    public class BridgeExtension : IBridgeExtension
    {
        /// <summary>
        /// Gets or sets a value indicating whether is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets order.
        /// </summary>
        public int Order => int.MaxValue;

        /// <summary>
        /// Format log entry.
        /// </summary>
        /// <param name="logRequest">reference to log request</param>
        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            ////var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            this.Handled = true;
        }
    }
}
