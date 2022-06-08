namespace EuQueroApp.Infraestrutura.Dados;

public class QueryObterProdutosVendidos
{
    private readonly IConfiguration configuration;

    public QueryObterProdutosVendidos(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<ProdutoVendidosResponse>> Execute()
    {
        var db = new SqlConnection(configuration["ConnectionString:EuQueroDb"]);
        var query = @"SELECT 
                        P.[Id],
                        P.[Nome],
                        COUNT(*) Quantidade
                      FROM
                        [EuQueroDb].[dbo].[Pedidos] PE
                        INNER JOIN [EuQueroDb].[dbo].[PedidoProdutos] PP ON PP.[PedidosId] = PE.[Id]
                        INNER JOIN [EuQueroDb].[dbo].[Produtos] P ON P.[Id] = PP.[ProdutosId]
                      GROUP BY 
                        P.[Id], P.[Nome]
                      ORDER BY 
                        Quantidade DESC";

        return await db.QueryAsync<ProdutoVendidosResponse>(query);
    }
}
