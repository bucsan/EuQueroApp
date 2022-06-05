namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuarioGetAll
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        int? page, 
        int? rows, 
        QueryObterUsuariosClaimNome query)
    {
        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}
