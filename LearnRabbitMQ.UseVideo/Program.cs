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
            //这里尽量不要使用guest用户，不同用户可以接受同一个队列的消息
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "chenliang",
                Password = "123456"
            };
            //创建connection
            var connection = factory.CreateConnection();

            //创建channel
            var channel = connection.CreateModel();
            //exchange的默认type是direct
            //创建队列queue
            channel.QueueDeclare("mytest", false, true, false, null);

           var result=  channel.QueueDeclarePassive("mytest");

            //channel.QueueDeclareNoWait()
            //发布消息
            var message = Encoding.UTF8.GetBytes("Hello World!");
            channel.BasicPublish(exchange: "", routingKey: "mytest", basicProperties: null, body: message);

        }
    }
}
