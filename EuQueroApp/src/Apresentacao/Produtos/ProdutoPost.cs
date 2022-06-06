namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoPost
{
    public static string Template => "/produtos";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        ProdutoRequest produtoRequest, 
        HttpContext http, 
        ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == produtoRequest.CategoriaId);
        var produto = new Produto(produtoRequest.Nome, categoria, produtoRequest.Descricao, produtoRequest.EmEstoque, produtoRequest.Preco, userId);

        if (!produto.IsValid)
            return Results.ValidationProblem(produto.Notifications.ConvertToProblemDetails());

        await context.Produtos.AddAsync(produto);
        await context.SaveChangesAsync();

        return Results.Created($"/produtos/{produto.Id}", produto.Id);
    }
}
