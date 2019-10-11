using ICommandHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueuePublisher;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace RabbitQueueExtensions
{
    public static class RabbitQueueExtension
    {
        public static void AddSubscribeQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQ,
            ICommandHandler<T> handler)
        {
            var rabbitMQConfiguration = configuration.GetSection(configSectionRabbitMQ).Get<RabbitConfiguration>();

            AddSubscribeQueue(handler,
                rabbitMQConfiguration.HostName,
                rabbitMQConfiguration.Port,
                rabbitMQConfiguration.UserName,
                rabbitMQConfiguration.Password,
                rabbitMQConfiguration.ExchangeName,
                rabbitMQConfiguration.ExchangeType,
                rabbitMQConfiguration.RoutingKey,
                rabbitMQConfiguration.QueueName,
                rabbitMQConfiguration.NumberOfWorkroles);
        }

        public static void AddSubscribeQueue<T>(
            this IServiceCollection services,
            RabbitConfiguration rabbitMQConfiguration,
            ICommandHandler<T> handler)
        {
            AddSubscribeQueue(handler,
                rabbitMQConfiguration.HostName,
                rabbitMQConfiguration.Port,
                rabbitMQConfiguration.UserName,
                rabbitMQConfiguration.Password,
                rabbitMQConfiguration.ExchangeName,
                rabbitMQConfiguration.ExchangeType,
                rabbitMQConfiguration.RoutingKey,
                rabbitMQConfiguration.QueueName,
                rabbitMQConfiguration.NumberOfWorkroles);
        }

        public static void AddSubscribeQueue<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionRabbitMQHostConfiguration,
            string configSectionRabbitMQInfoQueueConfiguration,
            ICommandHandler<T> handler)
        {
            var rabbitMQHostConfiguration = configuration.GetSection(configSectionRabbitMQHostConfiguration).Get<RabbitHostConfiguration>();

            var rabbitMQInfoQueueConfiguration = configuration.GetSection(configSectionRabbitMQInfoQueueConfiguration).Get<RabbitInfoQueueConfiguration>();

            AddSubscribeQueue(handler,
                rabbitMQHostConfiguration.HostName,
                rabbitMQHostConfiguration.Port,
                rabbitMQHostConfiguration.UserName,
                rabbitMQHostConfiguration.Password,
                rabbitMQInfoQueueConfiguration.ExchangeName,
                rabbitMQInfoQueueConfiguration.ExchangeType,
                rabbitMQInfoQueueConfiguration.RoutingKey,
                rabbitMQInfoQueueConfiguration.QueueName,
                rabbitMQInfoQueueConfiguration.NumberOfWorkroles);
        }

        public static void AddSubscribeQueue<T>(
            this IServiceCollection services,
            RabbitHostConfiguration rabbitMQHostConfiguration,
            RabbitInfoQueueConfiguration rabbitMQInfoQueueConfiguration,
            ICommandHandler<T> handler)
        {
            AddSubscribeQueue(handler,
                rabbitMQHostConfiguration.HostName,
                rabbitMQHostConfiguration.Port,
                rabbitMQHostConfiguration.UserName,
                rabbitMQHostConfiguration.Password,
                rabbitMQInfoQueueConfiguration.ExchangeName,
                rabbitMQInfoQueueConfiguration.ExchangeType,
                rabbitMQInfoQueueConfiguration.RoutingKey,
                rabbitMQInfoQueueConfiguration.QueueName,
                rabbitMQInfoQueueConfiguration.NumberOfWorkroles);
        }

        private static void AddSubscribeQueue<T>(
            ICommandHandler<T> handler,
            string hostName,
            int port,
            string userName,
            string password,
            string exchangeName,
            string exchangeType,
            string routingKey,
            string queueName,
            int numberOfWorkroles)
        {
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
                          numberOfWorkroles)
                      );
        }
    }
}
