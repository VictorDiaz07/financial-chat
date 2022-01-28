using ChatBot.Core.Models;

namespace ChatBot.Core.Contracts
{
    public interface ISender
    {
        void SendMessage(ClientMessage message);
    }
}
