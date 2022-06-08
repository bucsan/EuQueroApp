namespace EuQueroApp.Dominio.Pedidos;

public class Pedido : Entity
{
    public string ClienteId { get; private set; }
    public List<Produto> Produtos { get; private set; }
    public decimal Total { get; private set; }
    public string EnderecoEntrega { get; private set; }

    private Pedido() { }

    public Pedido(string clienteId, string clienteNome, List<Produto> produtos, string enderecoEntrega)
    {
        ClienteId = clienteId;
        Produtos = produtos;
        EnderecoEntrega = enderecoEntrega;
        CriadoPor = clienteNome;
        DataCriacao = DateTime.Now;
        AtualizadoPor = clienteNome;
        DataAtualizacao = DateTime.Now;

        Total = 0;
        foreach (var item in Produtos)
        {
            Total += item.Preco;
        }

        Validate();
    }

    private void Validate()
    {
        var contrato = new Contract<Pedido>()
            .IsNotNull(ClienteId, "ClienteId", "Cliente não encontrado!")
            .IsTrue(Produtos != null && Produtos.Any(), "Produtos", "Produto é obrigatório para o pedido!")
            .IsNotEmpty(EnderecoEntrega, "EnderecoEntrega", "O endereço de entrega é obrigatório!");
        AddNotifications(contrato);
    }
}
