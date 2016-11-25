using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.RoutingSend
{
    class EmitLogDirect
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection=factory.CreateConnection())
            using (var channel=connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                while(true)
                {
                    Console.WriteLine("输入日志级别:日志内容，例如：info（success,danger）:***正常");
                    string message = Console.ReadLine();
                    string[] arrMsg = message.Split(':');

                    var body =Encoding.UTF8.GetBytes( arrMsg[1]);
                    channel.BasicPublish(exchange: "direct_logs", routingKey: arrMsg[0], basicProperties: null, body: body);

                    Console.WriteLine("[x] sent {0}",message);
                    Console.WriteLine("=========================");
                }

            }
        }
    }
}
