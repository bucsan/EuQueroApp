namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoGetAll
{
    public static string Template => "/produtos";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        ApplicationDbContext context)
    {
        var produtos = context.Produtos.Include(p => p.Categoria).OrderBy(p => p.Nome).ToList();
        var results = produtos.Select(p => new ProdutoResponse(p.Nome, p.Categoria.Nome, p.Descricao, p.EmEstoque, p.Preco, p.Ativo));
        return Results.Ok(results);
    }
}
