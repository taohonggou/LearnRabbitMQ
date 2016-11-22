using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRabbiMQ
{
    class Send
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localHost" };
            using (var connection=factory.CreateConnection())
            {
                using (var channel=connection.CreateModel())
                {

                }
            }
        }
    }
}
