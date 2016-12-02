using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearnRabbitMQ.PracticeReceive
{
    class Program
    {
        private static string[] RoutingKey = { "test","log" };
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "directExchange",
                                    type: "direct");
                var queueName = channel.QueueDeclare().QueueName;
                foreach (var item in RoutingKey)
                {
                    channel.QueueBind(queue: queueName, exchange: "directExchange", routingKey: item);
                }

                Console.WriteLine("Waiting for message");

                var consume = new EventingBasicConsumer(channel);
                consume.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var exchange = ea.Exchange;
                    var routingKey = ea.RoutingKey;

                    Console.WriteLine("Recevied a message:{0},exchange:{1},routingKey:{2}", message, exchange, routingKey);
                };

                channel.BasicConsume(queue: queueName, noAck: true, consumer: consume);

                Console.ReadLine();
            }
        }
    }
}
