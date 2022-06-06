namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoGetVitrine
{
    public static string Template => "/produtos/vitrine";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        int? page,
        int? row,
        string? orderBy,
        ApplicationDbContext context)
    {
        if(page == null)
            page = 1;
        if(row == null)
            row = 10;
        if (string.IsNullOrEmpty(orderBy))
            orderBy = "nome";

        var queryBase = context.Produtos.Include(p => p.Categoria)
            .Where(p => p.EmEstoque && p.Categoria.Ativo);

        if(orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Nome);
        else
            queryBase = queryBase.OrderBy(p => p.Preco);

        var queryFilter = queryBase.Skip((page.Value - 1) * row.Value).Take(row.Value);

        var produtos = queryFilter.ToList();

        var results = produtos.Select(p => new ProdutoResponse(p.Nome, p.Categoria.Nome, p.Descricao, p.EmEstoque, p.Preco, p.Ativo));
        return Results.Ok(results);
    }
}
