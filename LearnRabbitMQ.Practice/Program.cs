using LearnRabbitMQ.Helper;

namespace LearnRabbitMQ.Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQHelper.Send("测试消息", "test");

            RabbitMQHelper.Send("测试日志", "log");
        }
    }
}
