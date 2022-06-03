using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EuQueroApp.Apresentacao.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList();
        var funcionarios = new List<FuncionarioResponse>();
        foreach (var item in users)
        {
            var claims = userManager.GetClaimsAsync(item).Result;
            var claimName = claims.FirstOrDefault(c => c.Type == "Nome");
            var userName =  claimName != null ? claimName.Value : string.Empty;
            funcionarios.Add(new FuncionarioResponse(item.Email, userName));
        }

        return Results.Ok(funcionarios);
    }
}
