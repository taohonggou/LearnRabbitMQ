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
        public static void Main(string[] args)//根据空格来区分长度
        {
            Console.WriteLine("To exit press CTRL+C");

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "durable", durable: true, exclusive: false, autoDelete: false, arguments: null);//durable:队列持久化

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;//消息持久化

                string message ="NewTask Start";
                while (true)
                {
                    if (message != null)
                    {
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "", routingKey: "durable", basicProperties: properties, body: body);
                    }
                    Console.WriteLine("发送了消息：{0}", message);
                    message = Console.ReadLine();
                }
            }
        }
    }
}
