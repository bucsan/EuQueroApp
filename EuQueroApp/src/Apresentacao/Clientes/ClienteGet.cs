namespace EuQueroApp.Apresentacao.Clientes;

public class ClienteGet
{
    public static string Template => "/clientes";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext http)
    {
        var user = http.User;
        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Nome = user.Claims.First(c => c.Type == "Nome").Value,
            Cpf = user.Claims.First(c => c.Type == "Cpf").Value
        };

        return Results.Ok(result);
    }
}
