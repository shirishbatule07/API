
namespace Patheyam.Domain.Interfaces
{
    public interface IDataRepository
    {
        bool TryConnect(out string message);
    }
}
