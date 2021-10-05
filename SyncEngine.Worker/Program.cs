using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;
using SyncEngine.Domain;
using SyncEngine.Managers;
using SyncEngine.Messaging;
using SyncEngine.Processor;

namespace SyncEngine.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = GetConfiguration();
                    var messagesSettings = new MessagesSettings();
                    config.Bind("RabbitMQ", messagesSettings);
                    services.AddSingleton<MessagesSettings>(messagesSettings);
                    services.AddSingleton<IMessageService, MessageService>();
                    services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"));
                    services.AddSingleton<IFileManager, FileManager>();
                    services.AddSingleton<BatchProcessor>();
                    services.AddHostedService<FileConsumer>();
                    services.AddHostedService<RecordConsumer>();
                });

        private static IConfigurationRoot GetConfiguration()
            =>  new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)

                .Build();
    }
}
