namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuariosPost
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(UsuarioRequest usuarioRequest, HttpContext http, UserManager<IdentityUser> userManager)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var newUser = new IdentityUser { UserName = usuarioRequest.Email, Email = usuarioRequest.Email };
        var result = await userManager.CreateAsync(newUser, usuarioRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim> 
        {
            new Claim("UsuarioCodigo", usuarioRequest.UsuarioCodigo),
            new Claim("Nome", usuarioRequest.Nome),
            new Claim("CriadoPor", userId),
        };
                
        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/usuarios/{newUser.Id}", newUser.Id);
    }
}
