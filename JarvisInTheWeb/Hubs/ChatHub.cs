using Microsoft.AspNetCore.SignalR;

namespace JarvisInTheWeb.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _hubContext;

        public ChatHub(IHubContext<ChatHub> ctx)
        {
            _hubContext = ctx;
        }

        public async Task SendMessage(string user, string message)
        {
            Api.Api.Instance(_hubContext).Prompt(message);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
