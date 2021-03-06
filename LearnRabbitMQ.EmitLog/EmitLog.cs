﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.EmitLog
{
    class EmitLog
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);

                while (true)
                {
                    Console.WriteLine("输入日志内容");
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
                    Console.WriteLine("[x] sent {0}", message);
                }
            }


        }
    }
}
