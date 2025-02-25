using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSults.Application.Shared.DataBase;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "Erro ao definir chave de conexão para a base de dados.");

        _connectionString = connectionString;
    }

    public IDbConnection Create()
        => new SqlConnection(_connectionString);
}
