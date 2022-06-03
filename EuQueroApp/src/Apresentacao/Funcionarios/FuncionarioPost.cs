using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EuQueroApp.Apresentacao.Funcionarios;

public class FuncionarioPost
{
    public static string Template => "/funcionarios";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(FuncionarioRequest funcionarioRequest, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = funcionarioRequest.Email, Email = funcionarioRequest.Email };
        var result = userManager.CreateAsync(user, funcionarioRequest.Password).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim> 
        {
            new Claim("FuncionarioCodigo", funcionarioRequest.FuncionarioCodigo),
            new Claim("Nome", funcionarioRequest.Nome)
        };
                
        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/funcionarios/{user.Id}", user.Id);
    }
}
