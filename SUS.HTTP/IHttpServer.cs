using System;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace SUS.HTTP
{
    public interface IHttpServer
    {
        void AddRoute(string path, Func<HttpRequest, HttpResponse> action);

        Task Start(int port);
    }
}
