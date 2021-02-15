using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using CommandLine;
using SyncCtl.Verbs;
using SyncCtl.Services;

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
            var appConfig = new AppConfig();
            config.Bind("app", appConfig);
            
            var collection = new ServiceCollection();
            collection.AddSingleton<AppConfig>(appConfig);
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
                messageService.Enqueue($"ITERATION: {i} {options.Data}");
            }
            return 1;
        }
    }
}
