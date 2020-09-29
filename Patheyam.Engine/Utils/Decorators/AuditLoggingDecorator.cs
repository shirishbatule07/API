
namespace Patheyam.Engine.Utils
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Serilog;

    public sealed class AuditLoggingCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
         where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;

        public AuditLoggingCommandDecorator(ICommandHandler<TCommand, TResult> handler)
        {
            _handler = handler;
        }

        public async Task<TResult> Handle(TCommand command)
        {
            var commandJson = JsonConvert.SerializeObject(command);

            Log.Information($"In {command.GetType().Name}, input: {commandJson}");

            return await _handler.Handle(command).ConfigureAwait(false);
        }
    }

    public sealed class AuditLoggingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
         where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;

        public AuditLoggingQueryDecorator(IQueryHandler<TQuery, TResult> handler)
        {
            _handler = handler;
        }

        public Task<TResult> Handle(TQuery query)
        {
            var queryJson = JsonConvert.SerializeObject(query);

            Log.Information($"In {query.GetType().Name}, input: {queryJson}");

            return _handler.Handle(query);
        }
    }
}
