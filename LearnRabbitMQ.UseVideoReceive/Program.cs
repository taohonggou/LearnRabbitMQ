using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LearnRabbitMQ.UseVideoReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factroy = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };

            var connection = factroy.CreateConnection();

            var queue= connection.CreateModel();
            var result= queue.BasicGet("mytest", true);
            var msg = Encoding.UTF8.GetString(result.Body);
          //result.

        }
    }
}
