namespace EuQueroApp.Dominio.Produtos;

public class Produto : Entity
{
    public string Nome { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; }
    public string Descricao { get; private set; }
    public bool EmEstoque { get; private set; }
    public bool Ativo { get; private set; } = true;

    private Produto() { }

    public Produto(string nome, Categoria categoria, string descricao, bool emEstoque, string criadoPor)
    {
        Nome = nome;        
        Categoria = categoria;
        Descricao = descricao;
        EmEstoque = emEstoque;
        
        CriadoPor = criadoPor;
        AtualizadoPor = criadoPor;
        DataCriacao = DateTime.Now;
        DataAtualizacao = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contrato = new Contract<Produto>()
            .IsNotNullOrEmpty(Nome, "Nome", "Nome é obrigatório!")
            .IsGreaterOrEqualsThan(Nome, 3, "Nome", "Nome tem que ser maior que 3 caracteres!")
            .IsNotNull(Categoria, "Categoria", "Está categoria mão foi encontrada!")
            .IsNotNullOrEmpty(Descricao, "Descricao", "Descrição é obrigatório!")
            .IsGreaterOrEqualsThan(Descricao, 3, "Descricao", "Descrição tem que ser maior que 3 caracteres!")
            .IsNotNullOrEmpty(CriadoPor, "CriadoPor", "CriadoPor é obrigatório!")
            .IsNotNullOrEmpty(AtualizadoPor, "AtualizadoPor");
        AddNotifications(contrato);
    }
}
