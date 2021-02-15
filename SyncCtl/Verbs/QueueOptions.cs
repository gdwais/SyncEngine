using CommandLine;

namespace SyncCtl.Verbs
{
    [Verb("queue")]
    public class QueueOptions
    {
        [Option('d', "data", HelpText = "The basic data you would like to enqueue")]
        public string Data { get; set; }
    }
}