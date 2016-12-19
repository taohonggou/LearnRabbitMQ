using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LearnRabbitMQ.Worker
{
    class Worker
    {
        public static void Main(string [] args)
        {
            var factory=new ConnectionFactory() { HostName="localhost" };
            using (var connection=factory.CreateConnection())
            using (var channel=connection.CreateModel())
            {
                channel.QueueDeclare(queue: "durable1",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);//这个是只要当前的worker正在处理消息，queue就不会将详细发给他，会发给其他worker。
                Console.WriteLine("[*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received {0}，deliveryTag：{1}", message,ea.DeliveryTag);

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine("[x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);//使用ack时必须有此代码，要不worker处理消息失败，queue在继续转发此消息时会有问题
                    //这是手动告诉RabbitMQ消息处理完毕。
                };
                channel.BasicConsume(queue: "durable1", noAck: false, consumer: consumer);
                //noAck设置为true，每次RabbitMQ发送消息后就将消息从内存中删除了，这样consumer在处理消息的过程中失败，消息不会重新发送。ack(acknowledgement:确认)
                //ack就是consumer告诉RabbitMQ消息以及收到并且处理完毕，这是RabbitMQ就会删除此消息
                //如果一个小处理失败了，他会马上发给其他消费者，也就是会将失败的消息放到队列的头部

                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
