namespace EuQueroApp.Dominio.Produtos;

public class Categoria : Entity
{
    public string Nome { get; set; }
    public bool Ativo { get; set; } = true;
}
