using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.Helper
{
    /// <summary>
    /// RabbitMQ的帮助类
    /// </summary>
    public class RabbitMQHelper
    {
        public static bool Send(string message, string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "directExchange", type: "direct");
                string queueName = channel.QueueDeclare().QueueName;
                //channel.QueueDeclare(queue:"")

                //while (true)
                //{
                //    Console.WriteLine("输入日志级别:日志内容，例如：info（success,danger）:***正常");
                //    string message = Console.ReadLine();
                //    string[] arrMsg = message.Split(':');

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "directExchange", routingKey: routingKey, basicProperties: null, body: body);

                return true;
                //    Console.WriteLine("[x] sent a message,routingkey:{0},message:{1}", arrMsg[0], arrMsg[1]);
                //    Console.WriteLine("=========================");
                //}

            }
        }
    }
}
