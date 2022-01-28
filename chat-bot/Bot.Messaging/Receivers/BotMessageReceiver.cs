using ChatBot.Core;
using ChatBot.Core.Contracts;
using ChatBot.Core.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Messaging.Receivers
{
    public class BotMessageReceiver : BackgroundService
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly string _listenToQueueName;
        private readonly IBot _bot;
        private readonly ISender _sender;
        private IConnection _connection;
        private IModel _channel;

        public BotMessageReceiver(IOptions<RabbitMqConfiguration> rabbitMqOptions, IBot bot, ISender sender)
        {
            _hostname = rabbitMqOptions.Value.HostName;
            _password = rabbitMqOptions.Value.Password;
            _username = rabbitMqOptions.Value.UserName;
            _listenToQueueName = rabbitMqOptions.Value.ListenToQueueName;
            _bot = bot;
            _sender = sender;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _listenToQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        private void HandleMessage(ClientMessage message)
        {
            try
            {
                StockQuote quote = _bot.GetStockQuote(message);

                if (quote != null)
                {
                    _sender.SendMessage(GetStockQuoteBotMessage(quote));
                }
            }
            catch (Exception)
            {
                _sender.SendMessage(new ClientMessage
                {
                    ClientUserName = "#BOT",
                    SendedOnUtc = DateTime.Now,
                    Message = $"Could not get stock quote."
                });
            }
        }

        private ClientMessage GetStockQuoteBotMessage(StockQuote quote)
        {
            return new ClientMessage
            {
                ClientUserName = "#BOT",
                SendedOnUtc = DateTime.Now,
                Message = $"{quote.Symbol} quote is ${quote.Close} per share"
            };
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e) { }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var clientMessage = JsonConvert.DeserializeObject<ClientMessage>(content);

                HandleMessage(clientMessage);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_listenToQueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
