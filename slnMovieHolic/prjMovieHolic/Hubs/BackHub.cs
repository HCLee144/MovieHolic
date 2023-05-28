using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Metrics;

namespace prjMovieHolic.Hubs
{
    public class BackHub : Hub
    {

        public long getCounter()  //後檯登入時取得當下counter
        {
            return FrontHub.counter;
        }

    }
}
