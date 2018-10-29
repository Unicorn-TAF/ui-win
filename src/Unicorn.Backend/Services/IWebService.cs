namespace Unicorn.Backend.Services
{
    public interface IWebService
    {
        HttpResponse SendRequest(string requestBody);

        HttpResponse SendRequestAndDecompress(string requestBody);
    }
}
