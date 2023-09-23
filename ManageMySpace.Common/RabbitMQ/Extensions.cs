using ManageMySpace.Common.Commands;
using ManageMySpace.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.vNext;
using System.Reflection;


namespace ManageMySpace.Common.RabbitMQ
{
    public static class Extensions
    {

        public static ISubscription WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>((msg, ctx) => handler.HandleAsync(msg), 
                ctx => ctx.WithQueue(q => q.WithName(GetQueueName<TCommand>())));

        public static ISubscription WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>((msg, ctx) => handler.HandleAsync(msg),
                ctx => ctx.WithQueue(q => q.WithName(GetQueueName<TEvent>())));


        private static string GetQueueName<T>() => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var client = BusClientFactory.CreateDefault(options);
            services.AddSingleton<IBusClient>(client);
        }
    }
}
