using Dapper;
using EuQueroApp.Apresentacao.Funcionarios;
using Microsoft.Data.SqlClient;

namespace EuQueroApp.Infraestrutura.Dados;

public class QueryObterFuncionariosClaimNome
{
    private readonly IConfiguration configuration;

    public QueryObterFuncionariosClaimNome(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<FuncionarioResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:EuQueroDb"]);
        var query = @"SELECT 
                        U.[Email], 
                        C.[ClaimValue] AS Nome
                    FROM
                        AspNetUsers U
                        INNER JOIN AspNetUserClaims C ON U.Id = C.UserId
                                                      AND ClaimType = 'Nome'
                    ORDER BY 
                        Nome
                    OFFSET(@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        return db.Query<FuncionarioResponse>(query, new { page, rows });
    }
}
