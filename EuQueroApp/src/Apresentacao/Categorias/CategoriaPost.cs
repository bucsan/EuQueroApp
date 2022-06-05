using System.Security.Claims;
using EuQueroApp.Dominio.Produtos;
using EuQueroApp.Infraestrutura.Dados;
using Microsoft.AspNetCore.Authorization;

namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPost
{
    public static string Template => "/categorias";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static IResult Action(CategoriaRequest categoriaRequest, HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = new Categoria(categoriaRequest.Nome, userId, userId);

        if (!categoria.IsValid)
        {            
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());
        }            

        context.Categorias.Add(categoria);
        context.SaveChanges();

        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
