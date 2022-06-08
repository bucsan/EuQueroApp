namespace EuQueroApp.Apresentacao.Pedidos;

public class PedidoPost
{
    public static string Template => "/pedidos";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(
        PedidoRequest pedidoRequest, 
        HttpContext http, 
        ApplicationDbContext context)
    {
        var clienteId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clienteNome = http.User.Claims.First(c => c.Type == "Nome").Value;

        List<Produto> produtosEncontrados = null;

        if (pedidoRequest.ProdutosIds != null || pedidoRequest.ProdutosIds.Any())
            produtosEncontrados = context.Produtos.Where(p => pedidoRequest.ProdutosIds.Contains(p.Id)).ToList();   
        
        var pedido = new Pedido(clienteId, clienteNome, produtosEncontrados, pedidoRequest.EnderecoEntrega);
        if (pedido.IsValid)
            return Results.ValidationProblem(pedido.Notifications.ConvertToProblemDetails());

        await context.Pedidos.AddAsync(pedido);
        await context.SaveChangesAsync();  

        return Results.Created($"/pedidos/{pedido.Id}", pedido.Id);
    }
}
