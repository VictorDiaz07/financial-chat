using Chat.Services.Contracts;
using ChatBot.Core.Contracts;
using ChatBot.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Services.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ISender _sender;
        private static IList<ConnectedUser> _connectedUsers = new List<ConnectedUser>();
        private readonly IMessageService _messageService;

        public ChatHub(ISender sender, IMessageService messageService)
        {
            _sender = sender;
            _messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            if (!_connectedUsers.Any(connectedUser => connectedUser.UserName == userName))
            {
                _connectedUsers.Add(new ConnectedUser { ConnectionId = connectionId, UserName = userName });
                Clients.All.SendAsync("ConnectedUsersChanged", _connectedUsers);
            }
            else
            {
                Clients.Caller.SendAsync("ConnectedUsersChanged", _connectedUsers);
            }

            Clients.Caller.SendAsync("CurrentMessages", _messageService.GetTopMessages());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(ClientMessage message)
        {
            await Clients.All.SendAsync("NewMessage", message);

            if (message.Message.StartsWith("/stock="))
            {
                _sender.SendMessage(message);
                return;
            }

            await _messageService.Add(message);
        }

        public async Task DisconnectUser(string userName)
        {
            if (_connectedUsers.Any(currentUser => currentUser.UserName == userName))
            {
                _connectedUsers = _connectedUsers.Where(currentUser => currentUser.UserName != userName).ToList();
                await Clients.All.SendAsync("ConnectedUsersChanged", _connectedUsers);
            }
        }
    }
}
