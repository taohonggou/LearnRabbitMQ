using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearnRabbitMQ.TopicsReceive
{
    class ReceiveLogsTopic
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs1", type: "topic");
                //channel.ExchangeDeclare(exchange: "topic_logs2", type: "topic");

                //var queueName = channel.QueueDeclare().QueueName;
                var queueName = channel.QueueDeclare("test#",false,false,false,null).QueueName;
                if (args.Length < 1)
                {
                    Console.WriteLine("输入bindingkey，例如：*.C#.");
                    //Console.Error.WriteLine("Usage: {0} [binding_key...]",
                    //                        Environment.GetCommandLineArgs()[0]);
                    //Console.WriteLine(" Press [enter] to exit.");
                    //Console.ReadLine();
                    //Environment.ExitCode = 1;
                    string bindingkey = Console.ReadLine();
                    args = bindingkey.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    //return;
                }

                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "topic_logs1",
                                      routingKey: bindingKey);
                    //channel.QueueBind(queue: queueName,
                    //                exchange: "topic_logs2",
                    //                routingKey: bindingKey);
                }

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey,
                                      message);
                };
                channel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
