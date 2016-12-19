using RabbitMQ.Client;
using System;
using System.Text;

namespace LearnRabbiMQ.Send
{
    class Send
    {
        private static readonly Uri url =new Uri( "amqp://chenliang:123@192.168.0.80:5672/");
        static void Main()
        {
            var factory = new ConnectionFactory
            {
                uri = url
                //HostName = "192.168.0.80",
                //UserName = "chenliang",
                //Password = "123"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "HelloWorld",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                string message = "Hello World";
                while (true)
                {
                    if (string.IsNullOrEmpty(message) || message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        return;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "HelloWorld1", basicProperties: null, body: body);

                    Console.WriteLine("[X] send {0}，可以继续输入", message);
                    message = Console.ReadLine();
                }
            }
        }
    }
}
