namespace EuQueroApp.Apresentacao.Pedidos;

public record PedidoResponse(Guid Id, string ClienteEmail, IEnumerable<PedidoProduto> Produtos, string EnderecoEntrega);

public record PedidoProduto(Guid Id, string Nome);
