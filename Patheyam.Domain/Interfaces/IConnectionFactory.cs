namespace Patheyam.Domain.Interfaces
{
    using System.Data;
    public interface IConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
