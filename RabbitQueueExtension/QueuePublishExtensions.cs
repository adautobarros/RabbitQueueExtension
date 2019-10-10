using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueuePublisher;

namespace RabbitConnectionFactoryExtensions
{
    public static class QueuePublishExtensions
    {
        public static void AddQueePublishScoped(this IServiceCollection services)
        {
            services.AddScoped<IQueuePublish, QueuePublish>();
        }

        public static void AddQueePublishScoped(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddScoped<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }

        public static void AddQueePublishTransient(this IServiceCollection services)
        {
            services.AddTransient<IQueuePublish, QueuePublish>();
        }
        public static void AddQueePublishTransient(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddTransient<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }

        public static void AddQueePublishSingleton(this IServiceCollection services)
        {
            services.AddSingleton<IQueuePublish, QueuePublish>();
        }
        public static void AddQueePublishSingleton(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddSingleton<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }
    }
}
