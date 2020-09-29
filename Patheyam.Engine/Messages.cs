
namespace Patheyam.Engine
{
    using Patheyam.Engine.Utils;
    using System;
    using System.Threading.Tasks;

    public sealed class Messages : IMessages
    {
        private readonly IServiceProvider _provider;

        public Messages(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task<T> Dispatch<T>(ICommand<T> command)
        {
            var type = typeof(ICommandHandler<,>);
            Type[] typeArgs = { command.GetType(), typeof(T) };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Task<T> result = handler.Handle((dynamic)command);

            return result;
        }

        public Task<T> Dispatch<T>(IQuery<T> query)
        {
            var type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Task<T> result = handler.Handle((dynamic)query);

            return result;
        }

    }
}
