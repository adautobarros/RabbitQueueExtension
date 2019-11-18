using RabbitMQ.Client;
using System.Collections.Generic;

namespace RabbitQueueExtensions
{
    internal static class IRabbitMqModelExtensions
    {
        public static Dictionary<string, object> CreateDeadLetterQueue(this IModel channel, string deadLetterExchange, string deadLetterRoutingKey, string deadLetterQueue)
        {
            channel.ExchangeDeclare(deadLetterExchange, "direct");
            channel.QueueDeclare(deadLetterQueue, true, false);
            channel.QueueBind(queue: deadLetterQueue,
                            exchange: deadLetterExchange,
                            routingKey: deadLetterRoutingKey);

            return new Dictionary<string, object>()
            {
                { "x-dead-letter-exchange", deadLetterExchange },
                { "x-dead-letter-routing-key", deadLetterRoutingKey }
            };
        }
    }
}
