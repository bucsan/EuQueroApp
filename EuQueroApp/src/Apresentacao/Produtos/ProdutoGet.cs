namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoGet
{
    public static string Template => "/produtos/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        [FromRoute] Guid id, 
        ApplicationDbContext context)
    {
        var produto = await context.Produtos.Where(p => p.Id == id).Include(p => p.Categoria).FirstOrDefaultAsync();  
        if (produto == null)
            return Results.NotFound();
        
        var results = new ProdutoResponse(produto.Id, produto.Nome, produto.Categoria.Nome, produto.Descricao, produto.EmEstoque, produto.Preco, produto.Ativo);
        return Results.Ok(results);
    }
}
