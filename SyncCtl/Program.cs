using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;
using SyncCtl.Verbs;
using SyncEngine.Core;
using SyncEngine.Core.Messages;
using SyncEngine.Core.Configuration;

namespace SyncCtl
{
    class Program
    {
        private static ServiceProvider serviceProvider { get; set; }

        public static async Task<int> Main(string[] args)
        {
            RegisterServices();

            var parser = new Parser(settings => 
            {
                settings.CaseInsensitiveEnumValues = true;
                settings.HelpWriter = Console.Error;
            });

            return await parser.ParseArguments<QueueOptions>(args)
            .MapResult
            (
                (QueueOptions options) => RunQueueProcess(options),
                err => Task.FromResult(-1)
            );
        }

        private static void RegisterServices() 
        {
            var config = GetConfiguration();
            var appSettings = new AppSettings();
            config.Bind("Application", appSettings);
            var messagesSettings = new MessagesSettings();
            config.Bind("RabbitMQ", messagesSettings);

            var collection = new ServiceCollection();
            collection.AddSingleton<AppSettings>(appSettings);
            collection.AddSingleton<MessagesSettings>(messagesSettings);
            collection.AddSingleton<IMessageService, MessageService>();
            serviceProvider = collection.BuildServiceProvider();
        }

        private static IConfigurationRoot GetConfiguration()
            =>  new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)

                .Build();

        private static async Task<int> RunQueueProcess(QueueOptions options)
        {
            var messageService = serviceProvider.GetService<IMessageService>();
            for (int i = 0; i < 5000; i++)
            {
                var record = new Record { DomainId = options.Data };
                messageService.Enqueue<Record>(record);
            }
            return 1;
        }
    }
}
