using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.Tasks
{
    class NewTask
    {
        public static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("请输入消息 exit退出");
                string message= Console.ReadLine();
                if (message == "exit")
                    return;
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "taskqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);

                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
            
            //Console.WriteLine("Press [enter] to exit");
            //Console.ReadLine();

        }

        private static string GetMessage(string [] args)
        {
            return args.Length > 0 ? string.Join(" ", args) : "Hello World";
        }
    }
}
