using Bot.Services;
using ChatBot.Core.Models;
using NUnit.Framework;
using System;

namespace BotServicesTests
{
    [TestFixture]
    public class BotServiceTest
    {
        [TestCase("/stock=AAPL.US")]
        [TestCase("/stock=ARSAUD")]
        [TestCase("/stock=AUDIDR")]
        [TestCase("/stock=BGNCZK")]
        [TestCase("/stock=CHFRON")]
        public void With_Valid_Command_Returns_StockQuote(string command)
        {
            var botService = new BotService();
            var message = new ClientMessage
            {
                Message = command
            };

            var response = botService.GetStockQuote(message);

            Assert.That(response, Is.InstanceOf(typeof(StockQuote)));
        }

        [TestCase("")]
        [TestCase("/stock=")]
        public void With_Invalid_Command_Returns_Null(string command)
        {
            var botService = new BotService();
            var message = new ClientMessage
            {
                Message = command
            };

            var response = botService.GetStockQuote(message);

            Assert.That(response, Is.Null);
        }


        [TestCase("/stock=AAPL")]
        [TestCase("/stock=HELLO WORD")]
        public void With_Non_Existing_Command_Returns_RenderException(string command)
        {
            var botService = new BotService();
            var message = new ClientMessage
            {
                Message = command
            };

            Assert.Throws<FormatException>(delegate { botService.GetStockQuote(message); });
        }
    }
}