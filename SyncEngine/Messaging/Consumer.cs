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
using SyncEngine.Domain;

namespace SyncEngine.Worker
{
    public class Consumer : BackgroundService
    {
        public ILogger<Consumer> logger;
        public IConnection connection;
        public IModel channel;
        public string QueueName;
        public MessagesSettings settings;
        public delegate bool MessageHandler(object message);
        public MessageHandler handler;

        public void InitRabbitMQ()
        {
            try 
            {
                logger.LogInformation("Consumer running at: {time}", DateTimeOffset.Now);
                ConnectionFactory factory = new ConnectionFactory() { HostName = settings.Host, Port = settings.Port };
                factory.UserName = settings.UserName;
                factory.Password = settings.Password;
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: QueueName,
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
            channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        // public void HandleMessage<T>(T message)
        // {
        //     logger.LogInformation($"consumer received {message.ToString()}");
        // }

        public void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)  {  }  
        public void OnConsumerUnregistered(object sender, ConsumerEventArgs e) {  }  
        public void OnConsumerRegistered(object sender, ConsumerEventArgs e) {  }  
        public void OnConsumerShutdown(object sender, ShutdownEventArgs e) {  }  
        public void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)  {  }  
    
        public override void Dispose()  
        {  
            channel.Close();  
            connection.Close();  
            base.Dispose();  
        }
    }
}