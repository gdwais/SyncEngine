using SyncEngine.Core.Configuration;
using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SyncEngine.Core
{
    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        private readonly string routeKey;

        public MessageService(MessagesSettings settings)
        {
            _factory = new ConnectionFactory()
            {
                HostName = settings.Host, 
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password
            };
            this.routeKey = settings.RouteKey;
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: this.routeKey, durable: false, exclusive: false, autoDelete: false, arguments: null);

        }

        public Task<bool> Enqueue<T>(T message)
        {
            String jsonified = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonified);
            _channel.BasicPublish(exchange: "", routingKey: routeKey, basicProperties: null, body: body);
            
            return Task.FromResult(true);
        }
    }
}