using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRabbiMQ
{
    class Send
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localHost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //http://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
                channel.QueueDeclare(queue: "Hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                string message = "Hello World";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "Hello", basicProperties: null, body: body);

                Console.WriteLine("[X] send {0}", message);

            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
