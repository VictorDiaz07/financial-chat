using ChatBot.Core.Models;

namespace ChatBot.Core.Contracts
{
    public interface IBot
    {
        StockQuote GetStockQuote(ClientMessage clientMessage);
    }
}
