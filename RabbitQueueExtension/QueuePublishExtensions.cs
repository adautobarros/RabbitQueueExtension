using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueuePublisher;

namespace RabbitQueueExtensions
{
    public static class QueuePublishExtensions
    {
        public static void AddQueePublishScoped(this IServiceCollection services, RabbitHostConfiguration rabbitMQConfiguration)
        {
            services.AddScoped<IQueuePublish>(c => new QueuePublish(rabbitMQConfiguration));
        }

        public static void AddQueePublishScoped(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IQueuePublish>(c => new QueuePublish(configuration));
        }

        public static void AddQueePublishScoped(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddScoped<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }

        public static void AddQueePublishTransient(this IServiceCollection services, RabbitHostConfiguration rabbitMQConfiguration)
        {
            services.AddTransient<IQueuePublish>(c => new QueuePublish(rabbitMQConfiguration));
        }

        public static void AddQueePublishTransient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IQueuePublish>(c => new QueuePublish(configuration));
        }

        public static void AddQueePublishTransient(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddTransient<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }

        public static void AddQueePublishSingleton(this IServiceCollection services, RabbitHostConfiguration rabbitMQConfiguration)
        {
            services.AddSingleton<IQueuePublish>(c => new QueuePublish(rabbitMQConfiguration));
        }

        public static void AddQueePublishSingleton(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IQueuePublish>(c => new QueuePublish(configuration));
        }

        public static void AddQueePublishSingleton(this IServiceCollection services, IConfiguration configuration, string configSectionRabbitMQ)
        {
            services.AddSingleton<IQueuePublish>(c => new QueuePublish(configuration, configSectionRabbitMQ));
        }
    }
}
