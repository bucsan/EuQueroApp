namespace EuQueroApp.Apresentacao.Clientes;

public class ClientePost
{
    public static string Template => "/clientes";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ClienteRequest clienteRequest,
        UsuariosCriar usuariosCriar)
    {
        var userClaims = new List<Claim>
        {
            new Claim("Cpf", clienteRequest.Cpf),
            new Claim("Nome", clienteRequest.Nome)
        };

        (IdentityResult identity, string userId) result = 
            await usuariosCriar.Criar(clienteRequest.Email, clienteRequest.Password, userClaims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
        
        return Results.Created($"/clientes/{result.userId}", result.userId);
    }
}
