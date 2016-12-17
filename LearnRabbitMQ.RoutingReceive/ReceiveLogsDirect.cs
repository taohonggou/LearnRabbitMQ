using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace LearnRabbitMQ.RoutingReceive
{
    class ReceiveLogsDirect
    {
        public static void Main(string[] args)
        {
            string[] severity = args;

            while (true)
            {
                if (severity.Length >0)
                {
                    break;
                }
                Console.WriteLine("输入此队列要接收的消息类型，例如：info，success，danger");
                severity = Console.ReadLine().Split(' ');
            }

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                    type: "direct");
                var queueName = channel.QueueDeclare().QueueName;
                foreach (var item in severity)
                {
                    channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: item);
                }

                Console.WriteLine("Waiting for message");

                var consume = new EventingBasicConsumer(channel);
                consume.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var exchange = ea.Exchange;
                    var routingKey = ea.RoutingKey;

                    Console.WriteLine("Recevied a message:{0},exchange:{1},routingKey:{2}",message,exchange,routingKey);
                };

                channel.BasicConsume(queue: queueName, noAck: true, consumer: consume);

                Console.ReadLine();
            }
        }

    }
}
