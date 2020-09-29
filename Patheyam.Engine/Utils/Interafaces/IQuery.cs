
namespace Patheyam.Engine.Utils
{
    using System.Threading.Tasks;
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
