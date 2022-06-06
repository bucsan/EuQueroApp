namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuariosPost
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        UsuarioRequest usuarioRequest, 
        HttpContext http, 
        UsuariosCriar usuariosCriar)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim>
        {
            new Claim("UsuarioCodigo", usuarioRequest.UsuarioCodigo),
            new Claim("Nome", usuarioRequest.Nome),
            new Claim("CriadoPor", userId),
        };

        (IdentityResult identity, string userId) result =
            await usuariosCriar.Criar(usuarioRequest.Email, usuarioRequest.Password, userClaims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
                
        return Results.Created($"/usuarios/{result.userId}", result.userId);
    }
}
