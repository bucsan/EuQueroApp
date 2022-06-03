using EuQueroApp.Infraestrutura.Dados;

namespace EuQueroApp.Apresentacao.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, QueryObterFuncionariosClaimNome query)
    {
        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}
