using Microsoft.AspNetCore.SignalR;


namespace prjMovieHolic.Hubs
{
    public class FrontHub:Hub
    {
        public static long counter=0;


        public override async Task OnConnectedAsync()
        {
            counter = counter + 1;//當用戶進入頁面在線人數加一
            await Clients.All.SendAsync("updateCounter", counter); //登入刷新人數
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? e)
        {
            counter = counter - 1;
            await Clients.All.SendAsync("updateCounter", counter); 
            await base.OnDisconnectedAsync(e);
        }

        public long getCounter()  //後檯登入時取得當下counter
        {
            return counter;
        }
    }
}
