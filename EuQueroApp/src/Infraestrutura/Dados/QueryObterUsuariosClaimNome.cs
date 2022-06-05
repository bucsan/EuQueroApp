namespace EuQueroApp.Infraestrutura.Dados;

public class QueryObterUsuariosClaimNome
{
    private readonly IConfiguration configuration;

    public QueryObterUsuariosClaimNome(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<UsuarioResponse>> Execute(int page, int rows)
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

        return await db.QueryAsync<UsuarioResponse>(query, new { page, rows });
    }
}
