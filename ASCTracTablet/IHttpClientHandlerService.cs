using System.Net.Http;

namespace ASCTracTablet
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}


