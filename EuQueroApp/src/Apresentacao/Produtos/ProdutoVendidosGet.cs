namespace EuQueroApp.Apresentacao.Produtos;

public class ProdutoVendidosGet
{
    public static string Template => "/produtos/vendidos";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(QueryObterProdutosVendidos query)
    {
        var result = await query.Execute();
        return Results.Ok(result);
    }
}
