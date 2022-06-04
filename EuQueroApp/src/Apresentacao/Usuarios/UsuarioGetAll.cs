using EuQueroApp.Infraestrutura.Dados;

namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuarioGetAll
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, QueryObterUsuariosClaimNome query)
    {
        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}
