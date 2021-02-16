using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SyncEngine.Core;
using Newtonsoft.Json;

namespace SyncCtl.Services
{
    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;

        public MessageService()
        {
            Console.WriteLine("about to connect to rabbitmq");

            _factory = new ConnectionFactory()
            {
                HostName = "localhost", 
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
        }

        public bool Enqueue(Record record)
        {
            String jsonified = JsonConvert.SerializeObject(record);
            var body = Encoding.UTF8.GetBytes(jsonified);
            _channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
            Console.WriteLine($" [x] Published {record.DomainId} to RabbitMQ");
            return true;
        }
    }
}