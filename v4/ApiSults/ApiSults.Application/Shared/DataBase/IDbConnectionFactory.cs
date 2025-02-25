using System.Data;

namespace ApiSults.Application.Shared.DataBase;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
