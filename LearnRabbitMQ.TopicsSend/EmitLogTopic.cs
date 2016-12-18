using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.TopicsSend
{
    class EmitLogTopic
    {
        public static void Main(string [] args)
        {
            var factory = new ConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs1", type: "topic");
                channel.ExchangeDeclare(exchange: "topic_logs2", type: "topic");
                string[] inputMsg=new string[2];
                string msg = "";
                while (true)
                {
                    Console.WriteLine("请输入department.language.name:msg，例如：backstage.C#.chenliang:小伙儿干的不错 or backstage.java.mushuai:穆帅牛逼");
                    msg = Console.ReadLine();
                    inputMsg = msg.Split(':');
                    if(inputMsg.Length!=2)
                    {
                        Console.WriteLine("消息输入有误");
                        return;
                    }
                    var body = Encoding.UTF8.GetBytes(inputMsg[1]);
                    channel.BasicPublish(exchange: "topic_logs1", routingKey: inputMsg[0], basicProperties: null, body: body);
                    channel.BasicPublish(exchange: "topic_logs2", routingKey: inputMsg[0], basicProperties: null, body: body);
                    Console.WriteLine("[x] sent a message:{0}", msg);
                    Console.WriteLine("================================");
                }

                
            }
        }
    }
}
