using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRabbitMQ._7RPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var property = channel.CreateBasicProperties();

                SimpleRpcClient client = new SimpleRpcClient(channel,new PublicationAddress(ExchangeType.Direct,string.Empty,"rpc_queue"));

                var bytes = client.Call(Encoding.UTF8.GetBytes("Hello World!!!"));

                var result = Encoding.UTF8.GetString(bytes);
                Console.WriteLine(result);
                Console.Read();
            }
        }
    }
}
