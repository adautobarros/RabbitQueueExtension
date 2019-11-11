using ICommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RabbitQueueExtensions
{
    internal static class RabbitConnectionFactoryExtensions
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

        internal static async Task SubscribeAsync<T>(this ConnectionFactory connectionFactory, ICommandHandler<T> handler, string exchangeName = "", string exchangeType = "direct", string routingKey = "", string queueName = "", int NUMBER_OF_WORKROLES = 1)
        {
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            for (int i = 0; i < NUMBER_OF_WORKROLES; i++)
            {
                await Task.Factory.StartNew(() =>
                {
                    lock (channel)
                    {
                        var consumer = new EventingBasicConsumer(channel)
                        {
                            ConsumerTag = Guid.NewGuid().ToString() // Tag de identificação do consumidor no RabbitMQ
                        };
                        consumer.Received += async (sender, ea) =>
                        {

                            var mensagem = Encoding.UTF8.GetString(ea.Body);
                            var entidade = JsonConvert.DeserializeObject<T>(mensagem);
                            //Envia ao Handler a mensagem
                            await handler.HandleAsync(entidade);
                            //Diz ao RabbitMQ que a mensagem foi lida com sucesso pelo consumidor
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                        };
                        //Registra os consumidor no RabbitMQ
                        channel.BasicConsume(queueName, false, consumer: consumer);
                    }
                });
            }
        }
    }
}
