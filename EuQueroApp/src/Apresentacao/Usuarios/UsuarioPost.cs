using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuariosPost
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(UsuarioRequest usuarioRequest, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = usuarioRequest.Email, Email = usuarioRequest.Email };
        var result = userManager.CreateAsync(user, usuarioRequest.Password).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim> 
        {
            new Claim("UsuarioCodigo", usuarioRequest.UsuarioCodigo),
            new Claim("Nome", usuarioRequest.Nome)
        };
                
        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/usuarios/{user.Id}", user.Id);
    }
}
