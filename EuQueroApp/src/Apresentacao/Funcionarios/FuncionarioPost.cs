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
            return Results.BadRequest(result.Errors.First());

        var claimResult = userManager.AddClaimAsync(user, new Claim("FuncionarioCodigo", funcionarioRequest.FuncionarioCodigo)).Result;

        if(!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        claimResult = userManager.AddClaimAsync(user, new Claim("Nome", funcionarioRequest.Nome)).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/funcionarios/{user.Id}", user.Id);
    }
}
