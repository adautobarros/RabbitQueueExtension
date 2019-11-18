using ICommandHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueuePublisher;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace RabbitQueueExtensions
{
    public static class RabbitQueueAssyncExtensions
    {
        public static void AddSubscribeAssyncQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQ,
            ICommandHandler<T> handler)
        {
            var rabbitMQConfiguration = configuration.GetSection(configSectionRabbitMQ).Get<RabbitConfiguration>();

            AddSubscribeAssyncQueue(handler,
                rabbitMQConfiguration.HostName,
                rabbitMQConfiguration.Port,
                rabbitMQConfiguration.UserName,
                rabbitMQConfiguration.Password,
                rabbitMQConfiguration.ExchangeName,
                rabbitMQConfiguration.ExchangeType,
                rabbitMQConfiguration.RoutingKey,
                rabbitMQConfiguration.QueueName,
                rabbitMQConfiguration.NumberOfWorkroles,
                rabbitMQConfiguration.CreateDeadLetterQueue);
        }

        public static void AddSubscribeAssyncQueue<T>(
            this IServiceCollection services,
            RabbitConfiguration rabbitMQConfiguration,
            ICommandHandler<T> handler)
        {
            AddSubscribeAssyncQueue(handler,
                rabbitMQConfiguration.HostName,
                rabbitMQConfiguration.Port,
                rabbitMQConfiguration.UserName,
                rabbitMQConfiguration.Password,
                rabbitMQConfiguration.ExchangeName,
                rabbitMQConfiguration.ExchangeType,
                rabbitMQConfiguration.RoutingKey,
                rabbitMQConfiguration.QueueName,
                rabbitMQConfiguration.NumberOfWorkroles,
                rabbitMQConfiguration.CreateDeadLetterQueue);
        }

        public static void AddSubscribeAssyncQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQHostConfiguration,
            string configSectionRabbitMQInfoQueueConfiguration,
            ICommandHandler<T> handler)
        {
            var rabbitMQHostConfiguration = configuration.GetSection(configSectionRabbitMQHostConfiguration).Get<RabbitHostConfiguration>();

            var rabbitMQInfoQueueConfiguration = configuration.GetSection(configSectionRabbitMQInfoQueueConfiguration).Get<RabbitInfoQueueConfiguration>();

            AddSubscribeAssyncQueue(handler,
                rabbitMQHostConfiguration.HostName,
                rabbitMQHostConfiguration.Port,
                rabbitMQHostConfiguration.UserName,
                rabbitMQHostConfiguration.Password,
                rabbitMQInfoQueueConfiguration.ExchangeName,
                rabbitMQInfoQueueConfiguration.ExchangeType,
                rabbitMQInfoQueueConfiguration.RoutingKey,
                rabbitMQInfoQueueConfiguration.QueueName,
                rabbitMQInfoQueueConfiguration.NumberOfWorkroles,
                rabbitMQInfoQueueConfiguration.CreateDeadLetterQueue);
        }

        public static void AddSubscribeAssyncQueue<T>(
            this IServiceCollection services,
            RabbitHostConfiguration rabbitMQHostConfiguration,
            RabbitInfoQueueConfiguration rabbitMQInfoQueueConfiguration,
            ICommandHandler<T> handler)
        {
            AddSubscribeAssyncQueue(handler,
                rabbitMQHostConfiguration.HostName,
                rabbitMQHostConfiguration.Port,
                rabbitMQHostConfiguration.UserName,
                rabbitMQHostConfiguration.Password,
                rabbitMQInfoQueueConfiguration.ExchangeName,
                rabbitMQInfoQueueConfiguration.ExchangeType,
                rabbitMQInfoQueueConfiguration.RoutingKey,
                rabbitMQInfoQueueConfiguration.QueueName,
                rabbitMQInfoQueueConfiguration.NumberOfWorkroles,
                rabbitMQInfoQueueConfiguration.CreateDeadLetterQueue);
        }

        private static void AddSubscribeAssyncQueue<T>(
            ICommandHandler<T> handler,
            string hostName,
            int port,
            string userName,
            string password,
            string exchangeName,
            string exchangeType,
            string routingKey,
            string queueName,
            int numberOfWorkroles,
            bool createDeadLetterQueue)
        {

            if (handler == null || string.IsNullOrEmpty(hostName)
                         || port < 1
                         || string.IsNullOrEmpty(userName)
                         || string.IsNullOrEmpty(password)
                         || string.IsNullOrEmpty(exchangeName)
                         || string.IsNullOrEmpty(exchangeType)
                         || string.IsNullOrEmpty(routingKey)
                         || string.IsNullOrEmpty(queueName)                         
                         || numberOfWorkroles < 1)
                throw new ArgumentException("A configuração do Subscribe do RabbitMQ está incorreta");


            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
            Task.Factory.StartNew(async () =>
                          await factory.SubscribeAsync(handler,
                          exchangeName,
                          exchangeType,
                          routingKey,
                          queueName,
                          numberOfWorkroles, createDeadLetterQueue)
                      );
        }
    }
}
