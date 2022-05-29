namespace EuQueroApp.Apresentacao.Categorias;

public record CategoriaResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
}
