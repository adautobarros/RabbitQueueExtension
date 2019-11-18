using ICommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitQueueExtensions
{
    internal static class RabbitConnectionFactorySubscribeAssyncExtensions
    {
        internal static async Task SubscribeAsync<T>(this ConnectionFactory connectionFactory, ICommandHandler<T> handler, string exchangeName, string exchangeType, string routingKey, string queueName, int numberOfWorkroles, bool createDeadLetterQueue)
        {
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
            connectionFactory.RequestedConnectionTimeout = 60000;
            Dictionary<string, object> args = null;
            if (createDeadLetterQueue)
                args = channel.CreateDeadLetterQueue($"{exchangeName}-dead", $"{routingKey}-dead", $"{queueName}-dead");


            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            for (int i = 0; i < numberOfWorkroles; i++)
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
                            try
                            {
                                var mensagem = Encoding.UTF8.GetString(ea.Body);
                                var entidade = JsonConvert.DeserializeObject<T>(mensagem);
                                //Envia ao Handler a mensagem
                                await handler.HandleAsync(entidade);
                                //Diz ao RabbitMQ que a mensagem foi lida com sucesso pelo consumidor
                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);


                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex);
                                channel.BasicNack(ea.DeliveryTag, false, false);
                            }

                        };
                        //Registra os consumidor no RabbitMQ
                        channel.BasicConsume(queueName, false, consumer: consumer);
                    }
                });
            }
        }
    }
}
