using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearnRabbitMQ.Receive
{
    class Receive
    {
        private static readonly Uri Url = new Uri("amqp://chenliang:123@192.168.0.80:5672/");
        public static void Main()
        {
            var factory = new ConnectionFactory()
            {
                uri=Url
                 /*HostName = "localHost"*/
            };
            using (var connect=factory.CreateConnection())
            using (var channel = connect.CreateModel())
            {
                channel.QueueDeclare(queue: "HelloWorld", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[X] received {0}", message);
                };
                channel.BasicConsume(queue: "HelloWorld", noAck: true, consumer: consumer);
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
