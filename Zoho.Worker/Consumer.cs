using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SyncEngine.Core;

namespace Zoho.Worker
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger<Consumer> logger;
        private IConnection connection;
        private IModel channel;

        public Consumer(ILogger<Consumer> logger)
        {
            this.logger = logger;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            try 
            {
                logger.LogInformation("Consumer running at: {time}", DateTimeOffset.Now);
                ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
                factory.UserName = "guest";
                factory.Password = "guest";
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: "hello",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received from Rabbit: {0}", message);
                };
                
            } 
            catch (Exception ex)
            {
                //it's fine
                Console.WriteLine("not yet....");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) => 
            {
                var jsonContent = Encoding.UTF8.GetString(ea.Body.ToArray());
                Record record = JsonConvert.DeserializeObject<Record>(jsonContent);
                HandleMessage(record);
                channel.BasicAck(ea.DeliveryTag, false);
                await Task.Yield();
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
            channel.BasicConsume(queue: "hello", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(Record record)
        {
            logger.LogInformation($"consumer received {record.DomainId}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)  {  }  
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) {  }  
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)  {  }  
    
        public override void Dispose()  
        {  
            channel.Close();  
            connection.Close();  
            base.Dispose();  
        }
    }
}
