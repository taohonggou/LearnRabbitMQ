using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace LearnRabbitMQ.ReceiveLog
{
    class ReceiveLogs
    {
        public static void Main()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", "fanout");

                var queueName = channel.QueueDeclare().QueueName;//临时队列，会自动删除

                channel.QueueBind(queue: queueName,exchange: "logs", routingKey: "");//这里的routingkey写不写跟bind没有关系

                Console.WriteLine("Waiting for logs……");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] {0}",message);
                };

                channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);

                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
