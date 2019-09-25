using System.Net;

namespace Unicorn.Backend.Services
{
    public interface ISession
    {
        void UpdateRequestWithSessionData(ref HttpWebRequest request);
    }
}
