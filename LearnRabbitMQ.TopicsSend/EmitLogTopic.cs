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
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

                string[] inputMsg=new string[2] ;
                string msg = "";
                while (true)
                {
                    Console.WriteLine("请输入topic：msg，例如：book.english:人类简史 or computer.i5:自己组装");
                    msg = Console.ReadLine();
                    inputMsg = msg.Split(':');
                    if(inputMsg.Length!=2)
                    {
                        Console.WriteLine("消息输入有误");
                        return;
                    }
                    var body = Encoding.UTF8.GetBytes(inputMsg[1]);
                    channel.BasicPublish(exchange: "topic_logs", routingKey: inputMsg[0], basicProperties: null, body: body);
                    Console.WriteLine("[x] sent a message:{0}", msg);
                    Console.WriteLine("================================");
                }

                
            }
        }
    }
}
