namespace Unicorn.Backend.Services
{
    public interface IWebService<T> where T : HttpResponse
    {
        T SendRequest(string requestBody);

        T SendRequestAndDecompress(string requestBody);
    }
}
