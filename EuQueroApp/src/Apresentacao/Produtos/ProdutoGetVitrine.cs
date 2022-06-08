namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoGetVitrine
{
    public static string Template => "/produtos/vitrine";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ApplicationDbContext context,
        int page = 1,
        int row = 10,
        string? orderBy = "nome")
    {
        if (row > 10)
            return Results.Problem(title: "Limite é de 10 registros por página!", statusCode: 400);

        var queryBase = context.Produtos.AsNoTracking().Include(p => p.Categoria)
            .Where(p => p.EmEstoque && p.Categoria.Ativo);

        if(orderBy == "nome")
            queryBase = queryBase.OrderBy(p => p.Nome);
        else if (orderBy == "preco")
            queryBase = queryBase.OrderBy(p => p.Preco);
        else
            return Results.Problem(title: "A ordenação de deve ser por Nome ou Preço!", statusCode: 400);

        var queryFilter = queryBase.Skip((page - 1) * row).Take(row);

        var produtos = queryFilter.ToList();

        var results = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Categoria.Nome, p.Descricao, p.EmEstoque, p.Preco, p.Ativo));
        return Results.Ok(results);
    }
}
