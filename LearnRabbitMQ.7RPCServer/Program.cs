using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRabbitMQ._7RPCServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {


                //channel.ExchangeDeclare("logs", "fanout");

                //var queueName = channel.QueueDeclare().QueueName;//临时队列，会自动删除

                channel.QueueDeclare("rpc_queue", false, false, false, null);

                Subscription subscript = new Subscription(channel, "rpc_queue");
                MySimpleRpcServer server = new MySimpleRpcServer(subscript);

                Console.WriteLine("server start……");
                server.MainLoop();
                Console.Read();
            }
        }
    }

    public class MySimpleRpcServer : SimpleRpcServer
    {
        public MySimpleRpcServer(Subscription subscript) : base(subscript)
        {

        }

        public override byte[] HandleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            return base.HandleCall(isRedelivered, requestProperties, body, out replyProperties);
        }

        public override byte[] HandleSimpleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            var msg = Encoding.UTF8.GetString(body);
            body = Encoding.UTF8.GetBytes($"消息的长度是:{msg.Length},消息的内容是：{msg}");
            return base.HandleSimpleCall(isRedelivered, requestProperties, body, out replyProperties);
        }

        public override void ProcessRequest(BasicDeliverEventArgs evt)
        {
            base.ProcessRequest(evt);
        }
    }
}
