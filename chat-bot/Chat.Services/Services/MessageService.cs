using Chat.Persistence.Entities;
using ChatBot.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Chat.Services.Contracts;
using ChatBot.Core.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.Services.Services
{
    public class MessageService : IMessageService
    {

        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<User> _userRepository;
        public MessageService(IRepository<Message> messageRepository, IRepository<User> userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }


        public async Task Add(ClientMessage message)
        {
            var client = _userRepository.Table.FirstOrDefault(x => x.UserName == message.ClientUserName);
            Message newMessage = new Message
            {
                Body = message.Message,
                CreatedOn = message.SendedOnUtc,
                User = client
            };
            
            await _messageRepository.InsertAsync(newMessage);
        }

        public IList<ClientMessage> GetTopMessages()
        {
            var messages = _messageRepository.Table.Include(x => x.User).OrderBy(message => message.CreatedOn).Take(50).ToList();

            var clientMessages = messages.Select(x => new ClientMessage { ClientUserName = x.User.UserName, SendedOnUtc = x.CreatedOn, Message = x.Body}).ToList();

            return clientMessages;
        }
    }
}
