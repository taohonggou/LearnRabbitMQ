using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.RoutingReceive
{
    class ReceiveLogsDirect
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection=factory.CreateConnection())
            using (var channel=connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                    type: "direct");

                var queueName = channel.QueueDeclare().QueueName;


            }
        }

    }
}
