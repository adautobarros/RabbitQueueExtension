﻿using Microsoft.Extensions.Configuration;
using QueuePublisher;
using RabbitMQ.Client;
using System;

namespace RabbitConnectionFactoryExtensions
{
    public class QueuePublish : IQueuePublish
    {
        private ConnectionFactory ConnectionFactory;
        private readonly IConfiguration _configuration;

        public QueuePublish(IConfiguration configuration)
        {
            Configurar(configuration, "RabbitMQConfiguration");
            _configuration = configuration;
        }

        public QueuePublish(IConfiguration configuration, string configSectionRabbitMQ)
        {
            _configuration = configuration;
            Configurar(configuration, configSectionRabbitMQ);
        }

        public QueuePublish(RabbitHostConfiguration rabbitMQConfiguration)
        {
            Configurar(rabbitMQConfiguration);
        }

        public void Configurar(IConfiguration configuration, string configSectionRabbitMQ)
        {
            Configurar(configuration.GetSection(configSectionRabbitMQ).Get<RabbitHostConfiguration>());
        }

        private void Configurar(RabbitHostConfiguration rabbitMQConfiguration)
        {
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

        public void Publish<T>(T entidade, string configPublishSectionRabbitMQ)
        {
            var info = _configuration.GetSection(configPublishSectionRabbitMQ).Get<RabbitInfoQueuePublushConfiguration>();
            ConnectionFactory.Publish(entidade, info.ExchangeName, info.ExchangeType, info.RoutingKey, info.QueueName);
        }

        public void Publish<T>(T entidade, RabbitInfoQueuePublushConfiguration info)
        {
            ConnectionFactory.Publish(entidade, info.ExchangeName, info.ExchangeType, info.RoutingKey, info.QueueName);
        }

        public void Publish<T>(T entidade, string exchangeName = "", string exchangeType = QueuePublisher.ExchangeType.Direct, string routingKey = "", string queueName = "")
        {
            ConnectionFactory.Publish(entidade, exchangeName, exchangeType, routingKey, queueName);
        }
    }
}
