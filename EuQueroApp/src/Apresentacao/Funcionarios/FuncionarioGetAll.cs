using Dapper;
using Microsoft.Data.SqlClient;

namespace EuQueroApp.Apresentacao.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
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

        var funcionarios = db.Query<FuncionarioResponse>(query, new {page, rows});

        return Results.Ok(funcionarios);
    }
}
