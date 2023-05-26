using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Metrics;

namespace prjMovieHolic.Hubs
{
    public class BackHub : Hub
    {
        //public async Task updateCounter()
        //{
        //    await Clients.All.SendAsync("updateCounter",FrontHub.counter);
        //}


        
        public long updateCounter() //目前無作用，前台與後台共用同一hub
        {
            return FrontHub.counter;
        }

    }
}
