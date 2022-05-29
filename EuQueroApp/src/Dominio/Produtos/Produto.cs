namespace EuQueroApp.Dominio.Produtos;

public class Produto : Entity
{
    public string Nome { get; set; }
    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
    public string Descricao { get; set; }
    public bool EmEstoque { get; set; }
    public bool Ativo { get; set; } = true;
}
