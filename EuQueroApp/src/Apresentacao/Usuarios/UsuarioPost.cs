using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace EuQueroApp.Apresentacao.Usuarios;

public class UsuariosPost
{
    public static string Template => "/usuarios";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static IResult Action(UsuarioRequest usuarioRequest, HttpContext http, UserManager<IdentityUser> userManager)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var newUser = new IdentityUser { UserName = usuarioRequest.Email, Email = usuarioRequest.Email };
        var result = userManager.CreateAsync(newUser, usuarioRequest.Password).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim> 
        {
            new Claim("UsuarioCodigo", usuarioRequest.UsuarioCodigo),
            new Claim("Nome", usuarioRequest.Nome),
            new Claim("CriadoPor", userId),
        };
                
        var claimResult = userManager.AddClaimsAsync(newUser, userClaims).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/usuarios/{newUser.Id}", newUser.Id);
    }
}
