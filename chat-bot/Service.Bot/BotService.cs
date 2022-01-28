using ChatBot.Core.Contracts;
using ChatBot.Core.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Bot.Services
{
    public class BotService : IBot
    {
        private string GetStockCodeFromMessage(string message)
        {
            var stockCode = string.Empty;
            var proccesor = new Regex(@"\/stock=(?<StockCode>.*)");
            Match matches = proccesor.Match(message);

            if (matches.Success)
                stockCode = matches.Groups["StockCode"].Value;

            return stockCode;
        }

        private IList<StockQuote> GetStockQuoteFromAPI(string stockCode)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            var response = (HttpWebResponse)request.GetResponse();

            TextReader reader = new StreamReader(response.GetResponseStream());
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<StockQuote>().ToList();

            return records;
        }

        public StockQuote GetStockQuote(ClientMessage clientMessage)
        {
            if (clientMessage == null)
                throw new ArgumentNullException($"The argument {nameof(clientMessage)} cannot be null.");

            string stockCode = GetStockCodeFromMessage(clientMessage.Message);

            if (!string.IsNullOrWhiteSpace(stockCode))
            {
                try
                {
                    IList<StockQuote> stockQuotes = GetStockQuoteFromAPI(stockCode);

                    if (stockQuotes != null && stockQuotes.Any())
                    {
                        return stockQuotes[0];
                    }
                }
                catch (Exception e)
                {
                    throw new FormatException(e.Message);
                }
            }

            return null;
        }
    }
}
