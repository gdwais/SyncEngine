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
    public class RecordConsumer : Consumer
    {

        public RecordConsumer(ILogger<RecordConsumer> logger, MessagesSettings settings)
        {
            this.settings = settings;
            this.logger = logger;
            this.QueueName = "Queue:ProcessRecord";
            InitRabbitMQ();
        }

        

    }
}
