using CRM.Models;
using Microsoft.AspNetCore.SignalR;

namespace CRM.Hubs
{
    public class UserHub : Hub
    {
        public async Task RefershUsers(IEnumerable<User> users)
        {
            await Clients.All.SendAsync("RefreshUsers", users);
        }
    }
}
