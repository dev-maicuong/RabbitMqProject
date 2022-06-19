using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerCore
{
    public static class HeaderExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Headers);
            channel.QueueDeclare("demo-header-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header = new Dictionary<string, object>()
            {
                { "account","new"}
            };

            channel.QueueBind("demo-header-queue", "demo-header-exchange", string.Empty, arguments: header);
            channel.BasicQos(0, 10, false);



            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Consumer 1: {message}");
            };

            channel.BasicConsume("demo-header-queue", true, consumer);
            Console.WriteLine("Consumer 1 started");
            Console.ReadLine();
        }
    }
}
