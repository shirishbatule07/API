
namespace Patheyam.Engine.Utils
{
    using System.Threading.Tasks;

    public interface ICommand<TResult>
    {
    }

    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }


}
