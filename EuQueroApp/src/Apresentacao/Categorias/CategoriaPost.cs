using EuQueroApp.Dominio.Produtos;
using EuQueroApp.Infraestrutura.Dados;

namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPost
{
    public static string Template => "/categorias";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = new Categoria
        {
            Nome = categoriaRequest.Nome,
            CriadoPor = "Teste",
            DataCriacao = DateTime.Now,
            AtualizadoPor = "Test",
            DataAtualizacao = DateTime.Now
        };
        context.Categorias.Add(categoria);
        context.SaveChanges();

        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
