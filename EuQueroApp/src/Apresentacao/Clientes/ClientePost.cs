namespace EuQueroApp.Apresentacao.Clientes;

public class ClientePost
{
    public static string Template => "/clientes";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ClienteRequest clienteRequest, 
        HttpContext http, 
        UserManager<IdentityUser> userManager)
    {
        var newUser = new IdentityUser { UserName = clienteRequest.Email, Email = clienteRequest.Email };
        var result = await userManager.CreateAsync(newUser, clienteRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim> 
        {
            new Claim("Cpf", clienteRequest.Cpf),
            new Claim("Nome", clienteRequest.Nome)            
        };
                
        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        return Results.Created($"/clientes/{newUser.Id}", newUser.Id);
    }
}
