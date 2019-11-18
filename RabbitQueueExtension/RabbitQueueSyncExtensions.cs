using ICommandHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueuePublisher;
using RabbitMQ.Client;
using System;

namespace RabbitQueueExtensions
{
    public static class RabbitQueueSyncExtensions
    {
        public static void AddSubscribeSyncQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQ,
            ICommandSyncHandler<T> handler)
        {
            var rabbitMQConfiguration = configuration.GetSection(configSectionRabbitMQ).Get<RabbitConfiguration>();

            AddSubscribeSyncQueue(handler,
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

        public static void AddSubscribeSyncQueue<T>(
            this IServiceCollection services,
            RabbitConfiguration rabbitMQConfiguration,
            ICommandSyncHandler<T> handler)
        {
            AddSubscribeSyncQueue(handler,
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

        public static void AddSubscribeSyncQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQHostConfiguration,
            string configSectionRabbitMQInfoQueueConfiguration,
            ICommandSyncHandler<T> handler)
        {
            var rabbitMQHostConfiguration = configuration.GetSection(configSectionRabbitMQHostConfiguration).Get<RabbitHostConfiguration>();

            var rabbitMQInfoQueueConfiguration = configuration.GetSection(configSectionRabbitMQInfoQueueConfiguration).Get<RabbitInfoQueueConfiguration>();

            AddSubscribeSyncQueue(handler,
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

        public static void AddSubscribeSyncQueue<T>(
            this IServiceCollection services,
            RabbitHostConfiguration rabbitMQHostConfiguration,
            RabbitInfoQueueConfiguration rabbitMQInfoQueueConfiguration,
            ICommandSyncHandler<T> handler)
        {
            AddSubscribeSyncQueue(handler,
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

        private static void AddSubscribeSyncQueue<T>(
            ICommandSyncHandler<T> handler,
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

            factory.SubscribeSync(handler,
                      exchangeName,
                      exchangeType,
                      routingKey,
                      queueName,
                      numberOfWorkroles, createDeadLetterQueue);
        }
    }
}
