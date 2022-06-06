namespace EuQueroApp.Dominio.Usuarios;

public class UsuariosCriar
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsuariosCriar(UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<(IdentityResult, string)> Criar(string email, string password, List<Claim> claims)
    {
        var newUser = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(newUser, password);

        if (!result.Succeeded)
            return (result, string.Empty);

        return (await _userManager.AddClaimsAsync(newUser, claims), newUser.Id);
    }
}
