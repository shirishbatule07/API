
namespace Patheyam.Engine.Utils
{
    using System.Threading.Tasks;

    public interface IMessages
    {
        Task<T> Dispatch<T>(ICommand<T> command);
        Task<T> Dispatch<T>(IQuery<T> query);
    }
}
