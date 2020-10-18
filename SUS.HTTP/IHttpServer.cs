using System;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace SUS.HTTP
{
    public interface IHttpServer
    {
        Task Start(int port);
    }
}
