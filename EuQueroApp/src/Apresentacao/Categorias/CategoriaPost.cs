using EuQueroApp.Dominio.Produtos;
using EuQueroApp.Infraestrutura.Dados;
using Microsoft.AspNetCore.Authorization;

namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPost
{
    public static string Template => "/categorias";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static IResult Action(CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = new Categoria(categoriaRequest.Nome, "Teste", "Teste");

        if (!categoria.IsValid)
        {            
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());
        }            

        context.Categorias.Add(categoria);
        context.SaveChanges();

        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
