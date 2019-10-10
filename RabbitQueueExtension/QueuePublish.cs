using Microsoft.Extensions.Configuration;
using QueuePublisher;
using RabbitMQ.Client;
using System;

namespace RabbitConnectionFactoryExtensions
{
    public class QueuePublish : IQueuePublish
    {
        private ConnectionFactory ConnectionFactory;

        public QueuePublish(IConfiguration configuration)
        {
            Configurar(configuration, "RabbitMQConfiguration");
        }

        public QueuePublish(IConfiguration configuration, string configSectionRabbitMQ)
        {
            Configurar(configuration, configSectionRabbitMQ);
        }

        public void Configurar(IConfiguration configuration, string configSectionRabbitMQ)
        {
            var rabbitMQConfiguration = configuration.GetSection(configSectionRabbitMQ).Get<RabbitHostConfiguration>();

            ValidarConfiguracao(rabbitMQConfiguration);

            ConnectionFactory = new ConnectionFactory()
            {
                HostName = rabbitMQConfiguration.HostName,
                Port = rabbitMQConfiguration.Port,
                UserName = rabbitMQConfiguration.UserName,
                Password = rabbitMQConfiguration.Password
            };
        }

        private static void ValidarConfiguracao(RabbitHostConfiguration rabbitMQConfiguration)
        {
            if (string.IsNullOrEmpty(rabbitMQConfiguration.HostName)
                            || string.IsNullOrEmpty(rabbitMQConfiguration.UserName)
                            || string.IsNullOrEmpty(rabbitMQConfiguration.Password)
                            || rabbitMQConfiguration.Port == 0)
                throw new ArgumentException("A configuração da section RabbitMQConfiguration do arquivo appsetings.json está incorreta");
        }

        public void Publish<T>(T entidade, string exchangeName = "", string exchangeType = QueuePublisher.ExchangeType.Direct, string routingKey = "", string queueName = "")
        {
            ConnectionFactory.Publish(entidade, exchangeName, exchangeType, routingKey, queueName);
        }
    }
}
