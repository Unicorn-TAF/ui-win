using System.Net;

namespace Unicorn.Backend.Services
{
    public interface ISessionInfo
    {
        void UpdateRequestWithSessionInfo(ref HttpWebRequest request);
    }
}
