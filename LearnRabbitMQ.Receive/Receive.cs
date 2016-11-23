using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearnRabbitMQ.Receive
{
    class Receive
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localHost" };
            using (var connect=factory.CreateConnection())
            using (var channel = connect.CreateModel())
            {
                channel.QueueDeclare(queue: "durableChannelOne", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[X] received {0}", message);
                };
                channel.BasicConsume(queue: "Hello", noAck: true, consumer: consumer);
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
