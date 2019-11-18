using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitQueueExtensions
{
    internal static class RabbitConnectionPublishExtensions
    {
        internal static void Publish<T>(this ConnectionFactory connectionFactory, T entidade = default, string exchangeName = "", string routingKey = "")
        {
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var message = JsonConvert.SerializeObject(entidade);
                var body = Encoding.UTF8.GetBytes(message);

                //Seta a mensagem como persistente
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                //Envia a mensagem para fila
                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: properties, body: body);
            }
        }
    }
}
