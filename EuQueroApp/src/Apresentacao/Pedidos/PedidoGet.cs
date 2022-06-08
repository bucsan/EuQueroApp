namespace EuQueroApp.Apresentacao.Pedidos;

public class PedidoGet
{
    public static string Template => "/pedidos/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action(
        Guid id, 
        HttpContext http, 
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        var clienteClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        var usuarioClaim = http.User.Claims.FirstOrDefault(c => c.Type == "UsuarioCodigo");

        var pedido = context.Pedidos.Include(o => o.Produtos).FirstOrDefault(p => p.Id == id);

        if (pedido.ClienteId != clienteClaim.Value && usuarioClaim == null)
            return Results.Forbid();

        var cliente = await userManager.FindByIdAsync(pedido.ClienteId);
        var produtosResponse = pedido.Produtos.Select(p => new PedidoProduto(p.Id, p.Nome));
        var pedidoResponse = new PedidoResponse(pedido.Id, cliente.Email, produtosResponse, pedido.EnderecoEntrega);

        return Results.Ok(pedidoResponse); 
    }
}
