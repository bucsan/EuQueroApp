namespace EuQueroApp.Apresentacao.Clientes;

public record PedidoRequest(List<Guid> ProdutosIds, string EnderecoEntrega);
