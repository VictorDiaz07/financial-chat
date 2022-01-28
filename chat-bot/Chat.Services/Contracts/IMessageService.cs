using Chat.Persistence.Entities;
using ChatBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Services.Contracts
{
    public interface IMessageService
    {
        Task Add(ClientMessage message);
        IList<ClientMessage> GetTopMessages();
    }
}
