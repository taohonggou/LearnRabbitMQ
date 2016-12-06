using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRabbitMQ.UseVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "datamip",
                Password = "123"
            };
            //创建connection
            var connection = factory.CreateConnection();

            //创建channel
            var channel = connection.CreateModel();
            //exchange的默认type是direct
            //创建队列queue
            channel.QueueDeclare("mytest", true, false, false, null);
            //发布消息
            var message = Encoding.UTF8.GetBytes("Hello World!");
            channel.BasicPublish(exchange: "", routingKey: "mytest", basicProperties: null, body: message);

        }
    }
}
